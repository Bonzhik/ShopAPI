using ShopAPI.Data;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;

namespace ShopAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddProduct(int[] categoryId, Product product)
        {

            foreach(var catId in categoryId)
            {
                var categoryProduct = new CategoryProduct()
                {
                    Category = _context.Categories.FirstOrDefault(c => c.Id == catId),
                    Product = product
                };
                _context.Add(categoryProduct);
            }
            _context.Add(product);
            return Save();

        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);

            return Save();
        }

        public Product GetProduct(int id)
        {
            return _context.Products.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
        }

        public Product GetProduct(string name)
        {
            return _context.Products.AsNoTracking().Where(p => p.Name == name).FirstOrDefault();
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products.AsNoTracking().OrderBy(p => p.Id).ToList();
        }

        public bool UpdateProduct(int[] catId, Product product)
        {
            var categoryProduct = _context.CategoriesProducts.Where(p => p.ProductId == product.Id).ToList();
            foreach (var unit in categoryProduct)
            {
                _context.Remove(unit);
            }

            foreach(int categoryId in catId)
            {
                CategoryProduct categoryProductEntity = new CategoryProduct()
                {
                    Category = _context.Categories.FirstOrDefault(c => c.Id == categoryId),
                    Product = product
                };
                _context.Update(categoryProductEntity);
            }

            _context.Update(product);

            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public ICollection<Product> GetProductsByCategory(Category category)
        {
            var products = _context.CategoriesProducts.Where(p => p.Category == category).AsNoTracking().ToList();
            List<Product> resultProducts = new List<Product>();
            foreach (var product in products)
            {
                resultProducts.Add(_context.Products.FirstOrDefault(p => p.Id == product.ProductId));
            }
            return resultProducts;
        }
    }
}

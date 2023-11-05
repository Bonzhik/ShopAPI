using ShopAPI.Models;

namespace ShopAPI.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int id);
        Product GetProduct(string name);
        public ICollection<Product> GetProductsByCategory(Category category);
        bool AddProduct(int[] categoryId, Product product);
        bool UpdateProduct(int[] categoryId, Product product);
        bool DeleteProduct(Product product);
        bool Save();

    }
}

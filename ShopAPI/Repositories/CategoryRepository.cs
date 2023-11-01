using ShopAPI.Data;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;

namespace ShopAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.AsNoTracking().OrderBy(c => c.Id).ToList();
        }

        public bool IsExists(Category category)
        {
            if (_context.Categories.AsNoTracking().Any(c => c.Name == category.Name))
            {
                return true;
            }
            return false;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
        public Category GetCategory(int categoryId)
        {
            return _context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == categoryId);
        }
        public bool CategoryNameExists(int categoryId, string categoryName)
        {
            return _context.Categories.AsNoTracking().Any(c => c.Id != categoryId && c.Name == categoryName);
        }
    }
}

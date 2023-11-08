using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;

namespace ShopAPI.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool CategoryNameExists(int categoryId, string categoryName);
        bool AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category catId);
        bool IsExists(Category category);
        ICollection<Category> GetCategoriesByProduct(int productId);
        bool Save();
    }
}

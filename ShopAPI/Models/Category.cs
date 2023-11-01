using ShopAPI.Models;

namespace ShopAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}

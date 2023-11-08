using ShopAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}

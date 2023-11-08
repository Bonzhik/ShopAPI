using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models
{
    public class Role
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}

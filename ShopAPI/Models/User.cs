using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [MinLength(1)]
        [StringLength(100)]
        public string Password { get; set; }

        public Role Role { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}

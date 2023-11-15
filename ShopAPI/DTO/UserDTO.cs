using ShopAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class UserDTO
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
    }
}

using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class RoleDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [StringLength(100)]
        public string Name { get; set; }
    }
}

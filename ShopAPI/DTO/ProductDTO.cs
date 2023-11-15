using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class ProductDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(1)]
        public string Description { get; set; }
        [Required]
        [Url]
        public string Image { get; set; }
        [Required]
        [Range(0, 100000)]
        public int Quantity { get; set; }
        [Required]
        [Range(0, 100000)]
        public int Price { get; set; }
    }
}

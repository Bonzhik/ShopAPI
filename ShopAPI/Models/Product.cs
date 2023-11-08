using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(1)]
        public string Name { get; set; }
        [Required ]
        [StringLength(100)]
        [MinLength(1)]
        public string Description {  get; set; }
        [Required]
        [Url]
        public string Image {  get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Range(1, 100000)]
        public int Price { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

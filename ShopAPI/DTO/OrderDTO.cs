using ShopAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class OrderDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
    }
}

namespace ShopAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

namespace ShopAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}

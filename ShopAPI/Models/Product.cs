﻿namespace ShopAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description {  get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

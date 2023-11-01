using ShopAPI.Data;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddOrder(int[][] productId, Order order)
        {
            _context.Add(order);
            for (int i = 0; i < productId.Length; i++)
            {
                var currentproduct = _context.Products.FirstOrDefault(p => p.Id == productId[i][0]);
                currentproduct.Quantity -= productId[i][1];

                OrderProduct orderProductEntity = new OrderProduct()
                {
                    Order = order,
                    Product = currentproduct,
                    Quantity = productId[i][1]
                };
                _context.Add(orderProductEntity);
            }
            return Save();
        }

        public bool DeleteOrder(Order order)
        {
            var productsInOrder = _context.OrdersProducts.AsNoTracking().Where(p => p.OrderId == order.Id).ToList();
            foreach (var product in productsInOrder)
            {
                var currentproduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.ProductId);
                currentproduct.Quantity += product.Quantity;
                _context.Update(currentproduct);
            }
            _context.Remove(order);
            return Save();
        }

        public ICollection<OrderProduct> GetOrder(int id)
        {
            return _context.OrdersProducts.Where(p => p.OrderId == id).ToList();
        }

        public ICollection<Order> GetOrders()
        {
            return _context.Orders.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Order> GetOrders(int userId)
        {
            return _context.Orders.Where(o => o.User.Id == userId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOrder(Order order)
        {
            _context.Update(order);
            return Save();
        }
    }
}

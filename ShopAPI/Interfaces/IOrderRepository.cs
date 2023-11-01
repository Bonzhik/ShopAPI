using AutoMapper.Configuration.Conventions;
using ShopAPI.Models;
using System.Diagnostics;

namespace ShopAPI.Interfaces
{
    public interface IOrderRepository
    {
        ICollection<Order> GetOrders();
        ICollection<OrderProduct> GetOrder(int id);
        ICollection<Order> GetOrders(int userId);
        bool AddOrder(int[][] productId, Order order);
        bool UpdateOrder(Order order);
        bool DeleteOrder(Order order);
        bool Save();

    }
}

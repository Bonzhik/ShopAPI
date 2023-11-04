﻿using ShopAPI.Models;

namespace ShopAPI.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        bool AddUser(User user);
        bool UpdateUser(int[] roleId, User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
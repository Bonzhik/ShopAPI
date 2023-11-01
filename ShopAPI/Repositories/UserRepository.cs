using ShopAPI.Data;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddUser(User user)
        {
            var roleEntity = _context.Roles.FirstOrDefault(r => r.Id == 1);
            UserRole userRoleEntity = new UserRole()
            {
                User = user,
                Role = roleEntity,
            };
            _context.Add(userRoleEntity);
            _context.Add(user);

            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);

            return Save();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.AsNoTracking().OrderBy(u => u.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(int[] roleId, User user)
        {
            var userRole = _context.UserRoles.AsNoTracking().Where(u => u.UserId == user.Id).ToList();
            foreach (var role in userRole)
            {
                _context.Remove(role);
            }

            foreach (int rId in roleId)
            {
                UserRole userRoleEntity = new UserRole()
                {
                    User = user,
                    Role = _context.Roles.FirstOrDefault(r => r.Id == rId)
                };
                _context.Add(userRoleEntity);
            }

            _context.Update(user);

            return Save();
        }

    }
}

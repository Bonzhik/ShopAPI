using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Interfaces;
using ShopAPI.Models;

namespace ShopAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddRole(Role role)
        {
            _context.Add(role);
            return Save();
        }

        public bool DeleteRole(Role role)
        {
            _context.Remove(role);

            return Save();
        }

        public ICollection<Role> GetRoles()
        {
            return _context.Roles.AsNoTracking().OrderBy(c => c.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRole(Role role)
        {
            _context.Update(role);
            return Save();
        }
        public Role GetRole(int id)
        {
            return _context.Roles.AsNoTracking().FirstOrDefault(c => c.Id == id);
        }
        public bool IsExists(Role role)
        {
            if (_context.Categories.AsNoTracking().Any(c => c.Name == role.Name))
            {
                return true;
            }
            return false;
        }
    }
}

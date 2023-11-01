using ShopAPI.Models;

namespace ShopAPI.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetRoles();
        Role GetRole(int id);
        bool AddRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(Role role);
        bool IsExists(Role role);
        bool Save();
    }
}

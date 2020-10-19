using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IRolesService
    {
        void InsertAll(List<Roles> rolesList);
        List<Roles> GetRoles();
        Roles GetRoleById(int id);
        void UpdateAll(List<Roles> rolesList);
        void InsertorUpdate(Roles role);
    }
}

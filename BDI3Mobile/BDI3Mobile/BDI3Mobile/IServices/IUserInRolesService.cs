using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IUserInRolesService
    {
        void InsertAll(List<UsersInRoles> rolesList);
        List<UsersInRoles> GetRoles();
        UsersInRoles GetUserRoleById(int id);
        void UpdateAll(List<UsersInRoles> rolesList);
        void InsertorUpdate(UsersInRoles role);
    }
}

using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IUsersPermissionService
    {
        void InsertAll(List<PermissionsOnUsers> userPermissionsList);
        List<PermissionsOnUsers> GetUserPermissions();
        PermissionsOnUsers GetUsersPermissionById(int id);
        void UpdateAll(List<PermissionsOnUsers> userPermissionsList);
        void InsertorUpdate(PermissionsOnUsers userPermission);
    }
}

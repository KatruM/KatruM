using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IUserPermissionService
    {
        Task InsertAllAsync(List<UserPermissions> userPermissionList);
        Task<List<UserPermissions>> GetUserPermissionsAsync();
        Task DeleteAllAsync();
        Task<bool> GetStudentEditPermissionsAsync();
    }
}

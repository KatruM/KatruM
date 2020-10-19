using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IPermissionService
    {
        Task InsertAllAsync(List<Permissions> permissionList);
        Task<List<Permissions>> GetPermissionsAsync();
        Task DeleteAllAsync();
    }
}

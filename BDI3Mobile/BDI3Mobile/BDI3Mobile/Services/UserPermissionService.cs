using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public UserPermissionService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<UserPermissions>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public async Task DeleteAllAsync()
        {
            try
            {
                await _sqlAsyncConnection.DeleteAllAsync<UserPermissions>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public Task<List<UserPermissions>> GetUserPermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetStudentEditPermissionsAsync()
        {
            try
            {
                return await _sqlAsyncConnection.Table<UserPermissions>().FirstOrDefaultAsync(p => p.PermissionId == 195) != null ? true : false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return false;
        }

        public async Task InsertAllAsync(List<UserPermissions> permissionList)
        {
            try
            {
                await _sqlAsyncConnection.InsertAllAsync(permissionList, typeof(UserPermissions));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

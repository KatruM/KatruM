using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public PermissionService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<Permissions>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public async Task DeleteAllAsync()
        {
            try
            {
                await _sqlAsyncConnection.DeleteAllAsync<Permissions>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public Task<List<Permissions>> GetPermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task InsertAllAsync(List<Permissions> permissionList)
        {
            try
            {
                await _sqlAsyncConnection.InsertAllAsync(permissionList, typeof(Permissions));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using BDI3Mobile.IServices;
using SQLite;
using Xamarin.Forms;
using BDI3Mobile.Models.DBModels;
using System.Linq;

namespace BDI3Mobile.Services
{
    public class UsersPermissionService : IUsersPermissionService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;

        public UsersPermissionService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<PermissionsOnUsers>();
        }

        public List<PermissionsOnUsers> GetUserPermissions()
        {
            try
            {
                return _sqlConnection.Table<PermissionsOnUsers>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public PermissionsOnUsers GetUsersPermissionById(int id)
        {
            try
            {
                return _sqlConnection.Table<PermissionsOnUsers>().ToList().FirstOrDefault(p => p.PermissionsOnUsersID == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<PermissionsOnUsers> userPermissionsList)
        {
            try
            {
                _sqlConnection.InsertAll(userPermissionsList, typeof(PermissionsOnUsers));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(PermissionsOnUsers userPermission)
        {
            try
            {
                var localUserPermission = _sqlConnection.Table<PermissionsOnUsers>().ToList().FirstOrDefault(p => p.PermissionsOnUsersID == userPermission.PermissionsOnUsersID);
                if (localUserPermission == null)
                {
                    _sqlConnection.Insert(userPermission);
                }
                else
                {
                    _sqlConnection.Update(userPermission);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<PermissionsOnUsers> userPermissionsList)
        {
            try
            {
                _sqlConnection.UpdateAll(userPermissionsList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

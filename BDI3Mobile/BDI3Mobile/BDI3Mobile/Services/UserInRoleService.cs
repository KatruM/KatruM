using System;
using System.Collections.Generic;
using BDI3Mobile.IServices;
using SQLite;
using Xamarin.Forms;
using BDI3Mobile.Models.DBModels;
using System.Linq;

namespace BDI3Mobile.Services
{
    public class UserInRoleService : IUserInRolesService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;

        public UserInRoleService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<UsersInRoles>();
        }

        public List<UsersInRoles> GetRoles()
        {
            try
            {
                return _sqlConnection.Table<UsersInRoles>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public UsersInRoles GetUserRoleById(int id)
        {
            try
            {
                return _sqlConnection.Table<UsersInRoles>().ToList().FirstOrDefault(p => p.UserId == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<UsersInRoles> rolesList)
        {
            try
            {
                _sqlConnection.InsertAll(rolesList, typeof(UsersInRoles));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(UsersInRoles role)
        {
            try
            {
                var localRole = _sqlConnection.Table<UsersInRoles>().ToList().FirstOrDefault(p => p.UserId == role.UserId);
                if (localRole == null)
                {
                    _sqlConnection.Insert(role);
                }
                else
                {
                    _sqlConnection.Update(role);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<UsersInRoles> rolesList)
        {
            try
            {
                _sqlConnection.UpdateAll(rolesList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

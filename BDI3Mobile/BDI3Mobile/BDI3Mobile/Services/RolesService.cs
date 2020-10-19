using System;
using System.Collections.Generic;
using System.Linq;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class RolesService : IRolesService 
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public RolesService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<Roles>();
        }

        public Roles GetRoleById(int id)
        {
            try
            {
                return _sqlConnection.Table<Roles>().ToList().FirstOrDefault(p => p.RoleId == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public List<Roles> GetRoles()
        {
            try
            {
                return _sqlConnection.Table<Roles>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<Roles> rolesList)
        {
            try
            {
                _sqlConnection.InsertAll(rolesList, typeof(Roles));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(Roles role)
        {
            try
            {
                var localRole = _sqlConnection.Table<Roles>().ToList().FirstOrDefault(p => p.RoleId == role.RoleId);
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

        public void UpdateAll(List<Roles> rolesList)
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

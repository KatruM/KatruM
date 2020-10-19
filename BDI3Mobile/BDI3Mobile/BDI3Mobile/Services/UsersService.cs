using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class UsersService : IUsersService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public UsersService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<Users>();
        }
        public Users GetUserByOfflineID(int userid)
        {
            try
            {
                return _sqlConnection.Table<Users>().ToList().FirstOrDefault(p => p.UserId == userid);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public Users GetUserByID(int userID)
        {
            try
            {
                return _sqlConnection.Table<Users>().ToList().FirstOrDefault(p => p.UserId == userID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public List<Users> GetUsers()
        {
            try
            {
                return _sqlConnection.Table<Users>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<Users> users)
        {
            try
            {
                _sqlConnection.InsertAll(users, typeof(Users));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertOrUpdate(Users user)
        {
            try
            {
                var localUser = _sqlConnection.Table<Users>().ToList().FirstOrDefault(p => p.UserId == user.UserId);
                if (localUser == null)
                {
                    user.DeviceId = user.UserId + "_" + Guid.NewGuid();
                    _sqlConnection.Insert(user);
                }
                else
                {
                    user.DeviceId = localUser.DeviceId;
                    _sqlConnection.Update(user);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<Users> users)
        {
            try
            {
                _sqlConnection.UpdateAll(users);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public Users GetUserByUserName(string UserName)
        {
            try
            {
                return _sqlConnection.Table<Users>().FirstOrDefault(p => p.UserName.ToLower() == UserName.ToLower());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
    }
}

using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class UserSyncService : IUserSyncService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public UserSyncService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<UserSyncTable>();
            _sqlConnection.CreateTable<ContentSyncData>();
        }
        public void DeleteSyncData()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentSyncData>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteUserSync(int downloadedBy)
        {
            throw new NotImplementedException();
        }

        public ContentSyncData GetContentSyncData(string contentType)
        {
            try
            {
                return _sqlConnection.Table<ContentSyncData>().ToList().FirstOrDefault(p => p.ContentType == contentType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public UserSyncTable GetUserSyncTable(int downloadedBy)
        {
            try
            {
                return _sqlConnection.Table<UserSyncTable>().Where(p => p.DownLoadedBy == downloadedBy).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void Insert(ContentSyncData contentSyncData)
        {
            try
            {
                _sqlConnection.Insert(contentSyncData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertUserSync(UserSyncTable userSyncTable)
        {
            try
            {
                _sqlConnection.Insert(userSyncTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void Update(ContentSyncData contentSyncData)
        {
            try
            {
                _sqlConnection.Update(contentSyncData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateUserSync(UserSyncTable userSyncTable)
        {
            try
            {
                _sqlConnection.Update(userSyncTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

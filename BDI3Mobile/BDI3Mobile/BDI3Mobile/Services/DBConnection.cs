using BDI3Mobile.IServices;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class DBConnection : IDBConnection
    {
        public SQLiteConnection GetConnection()
        {
            string sqlFilePath;
            if(Device.RuntimePlatform == Device.Android)
            {
                sqlFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            }
            else if(Device.RuntimePlatform == Device.iOS)
            {
                sqlFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            }
            else
            {
                sqlFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            }
            var connection = new SQLiteConnection(Path.Combine(sqlFilePath, App.SqlFilename), true);
            return connection;
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            string sqlFilePath;
            if (Device.RuntimePlatform == Device.Android)
            {
                sqlFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                sqlFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            }
            else
            {
                sqlFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            }
            var connection = new SQLiteAsyncConnection(Path.Combine(sqlFilePath, App.SqlFilename), true);
            return connection;
        }
    }
}

using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class UserLastConnectionActivityService : IUserLastConnectionActivityService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public UserLastConnectionActivityService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<UserLastConnectionActivity>();
        }
    }
}

using SQLite;

namespace BDI3Mobile.IServices
{
    public interface IDBConnection
    {
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
    }
}

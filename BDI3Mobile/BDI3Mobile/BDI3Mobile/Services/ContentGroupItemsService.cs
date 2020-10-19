using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentGroupItemsService : IContentGroupItemsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentGroupItemsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentGroupItem>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentGroupItems, typeof(ContentGroupItem));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentGroupItem> GetItems()
        {
            var ContentGroupItem = default(List<ContentGroupItem>);
            try
            {
                ContentGroupItem = _sqlConnection.Table<ContentGroupItem>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentGroupItem;
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentGroupItem>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public async Task<List<ContentGroupItem>> GetItemsAsync()
        {
            var ContentGroupItem = default(List<ContentGroupItem>);
            try
            {
                ContentGroupItem = await _sqlAsyncConnection.Table<ContentGroupItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentGroupItem;
        }
    }
}

using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentItemsService : IContentItemsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentItemsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentItem>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentItems, typeof(ContentItem));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentItem> GetItems()
        {
            var contentItems = default(List<ContentItem>);
            try
            {
                contentItems = _sqlConnection.Table<ContentItem>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentItems;
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentItem>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<ContentItem> contentItems)
        {
            try
            {
                _sqlConnection.UpdateAll(contentItems);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public async  Task<List<ContentItem>> GetItemsAsync()
        {
            var contentItems = default(List<ContentItem>);
            try
            {
                contentItems = await _sqlAsyncConnection.Table<ContentItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentItems;
        }
    }
}

using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentItemTallyService : IContentItemTallyService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentItemTallyService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentItemTally>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentItemTallies, typeof(ContentItemTally));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentItemTally> GetItems()
        {
            var ContentItemTally = default(List<ContentItemTally>);
            try
            {
                ContentItemTally = _sqlConnection.Table<ContentItemTally>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentItemTally;
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentItemTally>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public async Task<List<ContentItemTally>> GetItemsAsync()
        {
            var ContentItemTally = default(List<ContentItemTally>);
            try
            {
                ContentItemTally = await _sqlAsyncConnection.Table<ContentItemTally>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentItemTally;
        }
    }
}

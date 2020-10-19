using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentCategoryLevelsService : IContentCategoryLevelsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentCategoryLevelsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentCategoryLevel>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }

        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentCategoryLevel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<ContentCategoryLevel> GetItems()
        {
            var contentCategoryLevels = default(List<ContentCategoryLevel>);
            try
            {
                contentCategoryLevels = _sqlConnection.Table<ContentCategoryLevel>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentCategoryLevels;
        }

        public async Task<List<ContentCategoryLevel>> GetItemsAsync()
        {
            var contentCategoryLevels = default(List<ContentCategoryLevel>);
            try
            {
                contentCategoryLevels = await _sqlAsyncConnection.Table<ContentCategoryLevel>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentCategoryLevels;
        }

        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentCategoryLevels, typeof(ContentCategoryLevel));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

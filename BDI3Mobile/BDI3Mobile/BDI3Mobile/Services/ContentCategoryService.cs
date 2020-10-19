using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace BDI3Mobile.Services
{
    public class ContentCategoryService : IContentCategoryService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public List<ContentCategory> getContentCategories { get; set; } = new List<ContentCategory>();
        public ContentCategoryService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentCategory>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public List<ContentCategory> GetContentCategory()
        {
            var contentCategory = default(List<ContentCategory>);
            try
            {
                contentCategory = _sqlConnection.Table<ContentCategory>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentCategory;
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentCategories, typeof(ContentCategory));
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentCategory>();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public async Task<List<ContentCategory>> GetItemsAsync()
        {
            var contentCategoryItems = default(List<ContentCategory>);
            try
            {
                contentCategoryItems = await _sqlAsyncConnection.Table<ContentCategory>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentCategoryItems;
        }
    }
}

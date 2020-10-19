using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentRubricsService : IContentRubricsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentRubricsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
            _sqlConnection.CreateTable<ContentRubric>();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentRubrics, typeof(ContentRubric));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void UpdateAll(List<ContentRubric> model)
        {
            try
            {
                _sqlConnection.UpdateAll(model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentRubric> GetItems()
        {
            var contentRubics = default(List<ContentRubric>);
            try
            {
                contentRubics = _sqlConnection.Table<ContentRubric>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentRubics;
        }
        public async Task<List<ContentRubric>> GetItemsAsync()
        {
            var contentRubics = default(List<ContentRubric>);
            try
            {
                contentRubics = await _sqlAsyncConnection.Table<ContentRubric>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentRubics;
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentRubric>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

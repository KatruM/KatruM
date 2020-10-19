using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentRubricPointsService : IContentRubricPointsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentRubricPointsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentRubricPoint>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentRubricPoints, typeof(ContentRubricPoint));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void UpdateAll(List<ContentRubricPoint> contentRubricPoints)
        {
            try
            {
                _sqlConnection.UpdateAll(contentRubricPoints);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentRubricPoint> GetItems()
        {
            var contentRubics = default(List<ContentRubricPoint>);
            try
            {
                contentRubics = _sqlConnection.Table<ContentRubricPoint>().ToList();
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
                _sqlConnection.DeleteAll<ContentRubricPoint>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public async Task<List<ContentRubricPoint>> GetItemsAsync()
        {
            var contentRubics = default(List<ContentRubricPoint>);
            try
            {
                contentRubics = await _sqlAsyncConnection.Table<ContentRubricPoint>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentRubics;
        }
    }
}

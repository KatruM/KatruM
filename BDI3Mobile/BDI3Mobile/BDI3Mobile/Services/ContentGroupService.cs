using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentGroupService : IContentGroupService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentGroupService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
            _sqlConnection.CreateTable<ContentGroup>();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentGroups, typeof(ContentGroup));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentGroup> GetItems()
        {
            var ContentGroup = default(List<ContentGroup>);
            try
            {
                ContentGroup = _sqlConnection.Table<ContentGroup>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentGroup;
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentGroup>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public async Task<List<ContentGroup>> GetItemsAsync()
        {
            var ContentGroup = default(List<ContentGroup>);
            try
            {
                ContentGroup = await _sqlAsyncConnection.Table<ContentGroup>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentGroup;
        }
    }
}

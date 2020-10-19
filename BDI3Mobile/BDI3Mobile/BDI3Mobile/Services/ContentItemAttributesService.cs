using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentItemAttributesService : IContentItemAttributesService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentItemAttributesService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentItemAttribute>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentItemAttributes, typeof(ContentItemAttribute));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentItemAttribute> GetItems()
        {
            var contentItemsAttributes = default(List<ContentItemAttribute>);
            try
            {
                contentItemsAttributes = _sqlConnection.Table<ContentItemAttribute>().OrderBy(ci=>ci.contentItemAttributeId).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentItemsAttributes;
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentItemAttribute>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<ContentItemAttribute> contentItemsAttribute)
        {
            try
            {
                _sqlConnection.UpdateAll(contentItemsAttribute);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public async Task<List<ContentItemAttribute>> GetItemsAsyns()
        {
            var contentItemsAttributes = default(List<ContentItemAttribute>);
            try
            {
                contentItemsAttributes = await _sqlAsyncConnection.Table<ContentItemAttribute>().OrderBy(ci => ci.contentItemAttributeId).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentItemsAttributes;
        }
    }
}

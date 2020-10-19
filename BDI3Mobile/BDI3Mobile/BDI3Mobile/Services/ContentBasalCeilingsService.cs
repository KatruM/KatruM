using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentBasalCeilingsService : IContentBasalCeilingsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public ContentBasalCeilingsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentBasalCeilings>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }

        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentBasalCeilings>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<ContentBasalCeilings> GetItems()
        {
            var contentCategoryItems = default(List<ContentBasalCeilings>);
            try
            {
                contentCategoryItems = _sqlConnection.Table<ContentBasalCeilings>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentCategoryItems;
        }

        public void InserAll(List<ContentBasalCeilings> model)
        {
            try
            {
                if (model != null && model.Any())
                {
                    _sqlConnection.InsertAll(model, typeof(ContentBasalCeilings));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public async Task<List<ContentBasalCeilings>> GetItemsAsync()
        {
            var contentCategoryItems = default(List<ContentBasalCeilings>);
            try
            {
                contentCategoryItems = await _sqlAsyncConnection.Table<ContentBasalCeilings>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return contentCategoryItems;
        }
    }
}

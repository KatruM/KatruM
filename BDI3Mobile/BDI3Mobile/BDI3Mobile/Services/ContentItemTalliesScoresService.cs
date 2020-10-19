using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ContentItemTalliesScoresService : IContentItemTalliesScoresService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        private readonly SQLiteConnection _sqlConnection;
        public ContentItemTalliesScoresService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ContentItemTallyScore>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.contentItemTallyScores, typeof(ContentItemTallyScore));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<ContentItemTallyScore> GetItems()
        {
            var ContentItemTallyScore = default(List<ContentItemTallyScore>);
            try
            {
                ContentItemTallyScore = _sqlConnection.Table<ContentItemTallyScore>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentItemTallyScore;
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<ContentItemTallyScore>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public async Task<List<ContentItemTallyScore>> GetItemsAsync()
        {
            var ContentItemTallyScore = default(List<ContentItemTallyScore>);
            try
            {
                ContentItemTallyScore = await _sqlAsyncConnection.Table<ContentItemTallyScore>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return ContentItemTallyScore;
        }
    }
}

using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class AssessmentsService : IAssessmentsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;

        public AssessmentsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<Assessment>();
        }

        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<Assessment>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<Assessment> GetItems()
        {
            var assessments = default(List<Assessment>);
            try
            {
                assessments = _sqlConnection.Table<Assessment>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return assessments;
        }

        public void InserAll(ContentCategoriesModel model)
        {
            try
            {
                _sqlConnection.InsertAll(model.assessments, typeof(Assessment));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

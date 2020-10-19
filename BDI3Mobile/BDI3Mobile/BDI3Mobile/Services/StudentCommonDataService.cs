using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class StudentCommonDataService : IStudentCommonDataService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        private readonly SQLiteAsyncConnection _sqlAsyncConnection;
        public StudentCommonDataService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<AllComanDataModel>();
            _sqlAsyncConnection = _dbconnection.GetAsyncConnection();
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<AllComanDataModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<AllComanDataModel> GetStudentCommonData()
        {
            try
            {
                return _sqlConnection.Table<AllComanDataModel>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InertAll(List<AllComanDataModel> comanDataModels)
        {
            try
            {
                _sqlConnection.InsertAll(comanDataModels, typeof(AllComanDataModel));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

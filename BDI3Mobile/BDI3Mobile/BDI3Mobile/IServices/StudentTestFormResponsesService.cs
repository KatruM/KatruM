using System;
using System.Collections.Generic;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.IServices
{
    public class StudentTestFormResponsesService : IStudentTestFormResponsesService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public StudentTestFormResponsesService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<StudentTestFormResponses>();
        }
        public void DeleteAll(int LocalFormInstanceId)
        {
            try
            {
                _sqlConnection.Execute("Delete from StudentTestFormResponses where LocalFormInstanceId = " + LocalFormInstanceId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<StudentTestFormResponses> GetStudentTestFormResponses(int LocalInstanceId)
        {
            try
            {
                return _sqlConnection.Table<StudentTestFormResponses>().Where(p=> p.LocalFormInstanceId == LocalInstanceId).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<StudentTestFormResponses> lstStudentTestFormResponses)
        {
            try
            {
                _sqlConnection.InsertAll(lstStudentTestFormResponses, typeof(StudentTestFormResponses));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<StudentTestFormResponses> lstStudentTestFormResponses)
        {
            try
            {
                _sqlConnection.UpdateAll(lstStudentTestFormResponses);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

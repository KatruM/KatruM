using System;
using System.Collections.Generic;
using BDI3Mobile.IServices;
using SQLite;
using Xamarin.Forms;
using BDI3Mobile.Models.DBModels;
using System.Linq;

namespace BDI3Mobile.Services
{
    public class StudentFundingSourceService : IStudentFundingSourcesService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public StudentFundingSourceService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<StudentFundingSources>();
        }

        public StudentFundingSources GetStudentFundingSourceById(int id)
        {
            try
            {
                return _sqlConnection.Table<StudentFundingSources>().ToList().FirstOrDefault(p => p.ID == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public List<StudentFundingSources> GetStudentsFundingSource()
        {
            try
            {
                return _sqlConnection.Table<StudentFundingSources>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<StudentFundingSources> studentsFundingSourcesList)
        {
            try
            {
                _sqlConnection.InsertAll(studentsFundingSourcesList, typeof(StudentFundingSources));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(StudentFundingSources studentFundingSource)
        {
            try
            {
                var localStudentFundingSource = _sqlConnection.Table<StudentFundingSources>().ToList().FirstOrDefault(p => p.ID == studentFundingSource.ID);
                if (localStudentFundingSource == null)
                {
                    _sqlConnection.Insert(studentFundingSource);
                }
                else
                {
                    _sqlConnection.Update(studentFundingSource);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<StudentFundingSources> studentFundingSourcesList)
        {
            try
            {
                _sqlConnection.UpdateAll(studentFundingSourcesList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

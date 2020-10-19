using System;
using System.Collections.Generic;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.Services.DigitalTestRecordService
{
    public class ClinicalTestFormService : IClinicalTestFormService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;

        public ClinicalTestFormService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<StudentTestFormOverview>();
        }

        public void InsertTestForm(StudentTestFormOverview testform)
        {
            try
            {
                _sqlConnection.Insert(testform);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateTestForm(StudentTestFormOverview testform)
        {
            try
            {
                _sqlConnection.Update(testform);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateTestFormNote(string notes,int localtestformID)
        {
            try
            {
                var localRecord = _sqlConnection.Table<StudentTestFormOverview>().FirstOrDefault(p => p.LocalTestRecodId == localtestformID);
                if (localRecord != null)
                {
                    localRecord.notes = notes;
                    _sqlConnection.Update(localRecord);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<StudentTestFormOverview> GetStudentTestFormsByStudentID(int localStudentID)
        {
            try
            {
                return _sqlConnection.Table<StudentTestFormOverview>().Where(p => p.LocalStudentId == localStudentID).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public List<StudentTestFormOverview> GetStudentTestForms()
        {
            try
            {
                return _sqlConnection.Table<StudentTestFormOverview>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }


        public StudentTestFormOverview GetStudentTestFormsByID(int localInstanceID)
        {
            try
            {
                return _sqlConnection.Table<StudentTestFormOverview>().FirstOrDefault(p => p.LocalTestRecodId == localInstanceID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public void DeleteTestFormByLocalID(int LocalTestRecordID)
        {
            var deleteQuery = "Delete From StudentTestFormOverview where LocalTestRecodId = " + LocalTestRecordID;
            _sqlConnection.Execute(deleteQuery);
        }

        public void UpdateSyncStatus(int code, int LocalTestRecordID)
        {
            var updateQuery = "Update StudentTestFormOverview set SyncStausCode =" + code  +" where LocalTestRecodId = " + LocalTestRecordID;
            _sqlConnection.Execute(updateQuery);
        }
    }
}

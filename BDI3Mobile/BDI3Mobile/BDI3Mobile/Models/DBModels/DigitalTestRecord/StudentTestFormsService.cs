using BDI3Mobile.IServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BDI3Mobile.Models.DBModels.DigitalTestRecord
{
    public class StudentTestFormsService : IStudentTestFormsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;

        public StudentTestFormsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<StudentTestForms>();
        }

        public void DeleteAll(int LocalTestInstanceID)
        {
            try
            {
                _sqlConnection.Execute("Delete from StudentTestForms where LocalformInstanceId = " + LocalTestInstanceID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<StudentTestForms> GetStudentTestForms(int localtestformID)
        {
            try
            {
                return _sqlConnection.Table<StudentTestForms>().Where(p => p.LocalformInstanceId == localtestformID).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(IEnumerable<StudentTestForms> StudentTestForms)
        {
            try
            {
                _sqlConnection.InsertAll(StudentTestForms, typeof(StudentTestForms));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateRawScore(int? rawscore, int localtestformid,int ContentCategoryID)
        {
            try
            {
                var recordinOffline = _sqlConnection.Table<StudentTestForms>().FirstOrDefault(p => p.LocalformInstanceId == localtestformid && p.contentCategoryId == ContentCategoryID);
                if (recordinOffline != null)
                {
                    recordinOffline.rawScore = rawscore;
                    _sqlConnection.Update(recordinOffline);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(IEnumerable<StudentTestForms> StudentTestForms)
        {
            try
            {
                _sqlConnection.UpdateAll(StudentTestForms);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void UpdateTestFormDate(int LocalInstanceId,int ContentCategoryID,string TestDate)
        {
            try
            {
                var recordinOffline =  _sqlConnection.Table<StudentTestForms>().FirstOrDefault(p => p.LocalformInstanceId == LocalInstanceId && p.contentCategoryId == ContentCategoryID);
                if (recordinOffline != null)
                {
                    recordinOffline.testDate = TestDate;
                    _sqlConnection.Update(recordinOffline);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void UpdateTestFormNotes(int LocalInstanceId, int ContentCategoryID, string Notes)
        {
            try
            {
                var recordinOffline = _sqlConnection.Table<StudentTestForms>().FirstOrDefault(p => p.LocalformInstanceId == LocalInstanceId && p.contentCategoryId == ContentCategoryID);
                if (recordinOffline != null)
                {
                    recordinOffline.Notes = Notes;
                    _sqlConnection.Update(recordinOffline);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void ResetScoresSelectedIds(string ids, int LocalformInstanceId)
        {
            try
            {
                var updateQuery = "update StudentTestForms set rawScore = null where contentCategoryId in (" + ids + ") and LocalformInstanceId = " + LocalformInstanceId;
                _sqlConnection.Execute(updateQuery);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<StudentTestForms> GetStudentTestRecords(string LocalformInstanceIds)
        {
            try
            {
                return _sqlConnection.Table<StudentTestForms>().ToList().Where(p => LocalformInstanceIds.Contains(p.LocalformInstanceId + "")).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
    }
}

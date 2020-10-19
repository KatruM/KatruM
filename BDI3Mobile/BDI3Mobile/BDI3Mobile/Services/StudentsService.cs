using System;
using System.Collections.Generic;
using BDI3Mobile.IServices;
using SQLite;
using Xamarin.Forms;
using BDI3Mobile.Models.DBModels;
using System.Linq;

namespace BDI3Mobile.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;

        public StudentsService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<Students>();
        }
        public Students GetStudentById(int id)
        {
            try
            {
                return _sqlConnection.Table<Students>().ToList().FirstOrDefault(p => p.OfflineStudentID == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public Students GetStudentByChildUserID(string childUserID)
        {
            try
            {
                return _sqlConnection.Table<Students>().ToList().FirstOrDefault(p => p.UserId == childUserID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public List<Students> GetStudents()
        {
            try
            {
                return _sqlConnection.Table<Students>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public List<Students> GetStudentsByOfflineID(int addedby)
        {
            try
            {
                return _sqlConnection.Table<Students>().Where(p => p.DownloadedBy == addedby && p.isDeleteStatus == 0).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        
        public Students IsRecordExists(Students student)
        {
            try
            {
                var studentsFound = _sqlConnection.Table<Students>().FirstOrDefault(p => p.UserId == student.UserId);
                if (studentsFound != null)
                {
                    return studentsFound;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        
        public void InsertAll(List<Students> studentsList)
        {
            try
            {
                _sqlConnection.InsertAll(studentsList, typeof(Students));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(Students student)
        {
            try
            {
                var localStudent = _sqlConnection.Table<Students>().ToList().FirstOrDefault(p => p.OfflineStudentID == student.OfflineStudentID);
                if (localStudent == null)
                {
                    _sqlConnection.Insert(student);
                }
                else
                {
                    _sqlConnection.Update(student);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<Students> studentsList)
        {
            try
            {
                _sqlConnection.UpdateAll(studentsList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<Students>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public List<Students> GetStudentsByDownloaded(int downloadBy)
        {
            try
            {
                return _sqlConnection.Table<Students>().Where(p => p.DownloadedBy == downloadBy).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public void Update(Students studentsList)
        {
            try
            {
                _sqlConnection.Update(studentsList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteByDownloadedBy(int downloadedby)
        {
            var query = "Delete from Students where DownloadedBy=" + downloadedby;
            _sqlConnection.Execute(query);
        }
    }
}

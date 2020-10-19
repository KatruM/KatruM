using System;
using System.Collections.Generic;
using BDI3Mobile.IServices;
using SQLite;
using BDI3Mobile.Models.DBModels;
using Xamarin.Forms;
using System.Linq;

namespace BDI3Mobile.Services
{
    public class StudentRaceService : IStudentsRaceService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public StudentRaceService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<StudentRace>();
        }

        public StudentRace GetStudentRaceById(int id)
        {
            try
            {
                return _sqlConnection.Table<StudentRace>().ToList().FirstOrDefault(p => p.StudentRaceID == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public List<StudentRace> GetStudentsRace()
        {
            try
            {
                return _sqlConnection.Table<StudentRace>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<StudentRace> studentsRaceList)
        {
            try
            {
                _sqlConnection.InsertAll(studentsRaceList, typeof(StudentRace));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(StudentRace studentRace)
        {
            try
            {
                var localStudentRace = _sqlConnection.Table<StudentRace>().ToList().FirstOrDefault(p => p.StudentRaceID == studentRace.StudentRaceID);
                if (localStudentRace == null)
                {
                    _sqlConnection.Insert(studentRace);
                }
                else
                {
                    _sqlConnection.Update(studentRace);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<StudentRace> rolesList)
        {
            try
            {
                _sqlConnection.UpdateAll(rolesList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

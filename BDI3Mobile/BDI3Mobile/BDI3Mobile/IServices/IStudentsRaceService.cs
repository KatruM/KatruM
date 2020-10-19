using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IStudentsRaceService
    {
        void InsertAll(List<StudentRace> studentsRaceList);
        List<StudentRace> GetStudentsRace();
        StudentRace GetStudentRaceById(int id);
        void UpdateAll(List<StudentRace> rolesList);
        void InsertorUpdate(StudentRace role);
    }
}

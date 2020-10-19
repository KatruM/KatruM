using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IStudentsService
    {
        void InsertAll(List<Students> studentsList);
        List<Students> GetStudents();
        List<Students> GetStudentsByOfflineID(int offlineUserID);
        Students GetStudentByChildUserID(string childUserID);
        Students GetStudentById(int guid);
        Students IsRecordExists(Students student);
        void UpdateAll(List<Students> studentsList);
        void InsertorUpdate(Students _location);
        void DeleteAll();
        List<Students> GetStudentsByDownloaded(int downloadBy);
        void Update(Students studentsList);
        void DeleteByDownloadedBy(int downloadedby);
    }
}

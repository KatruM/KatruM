using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IStudentTestFormResponsesService
    {
        void InsertAll(List<StudentTestFormResponses> lstStudentTestFormResponses);
        void UpdateAll(List<StudentTestFormResponses> lstStudentTestFormResponses);
        void DeleteAll(int LocalFormInstanceId);
        List<StudentTestFormResponses> GetStudentTestFormResponses(int LocalInstanceId);

    }
}

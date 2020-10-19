using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IStudentFundingSourcesService
    {
        void InsertAll(List<StudentFundingSources> studentsFundingSourcesList);
        List<StudentFundingSources> GetStudentsFundingSource();
        StudentFundingSources GetStudentFundingSourceById(int id);
        void UpdateAll(List<StudentFundingSources> studentFundingSourcesList);
        void InsertorUpdate(StudentFundingSources studentFundingSource);
    }
}

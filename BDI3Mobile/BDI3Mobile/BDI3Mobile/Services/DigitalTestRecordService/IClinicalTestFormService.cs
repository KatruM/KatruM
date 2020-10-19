using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using System.Collections.Generic;

namespace BDI3Mobile.Services.DigitalTestRecordService
{
    public interface IClinicalTestFormService
    {
        void InsertTestForm(StudentTestFormOverview testform);
        void UpdateTestForm(StudentTestFormOverview testform);
        List<StudentTestFormOverview> GetStudentTestFormsByStudentID(int localSudentID);
        List<StudentTestFormOverview> GetStudentTestForms();
        void UpdateTestFormNote(string notes, int localtestformID);
        StudentTestFormOverview GetStudentTestFormsByID(int localInstanceID);
        void DeleteTestFormByLocalID(int LocalTestRecordID);
        void UpdateSyncStatus(int code, int LocalTestRecordID);
    }
}

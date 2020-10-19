using System.Collections.Generic;

namespace BDI3Mobile.Models.DBModels.DigitalTestRecord
{
    public interface IStudentTestFormsService
    {
        void InsertAll(IEnumerable<StudentTestForms> StudentTestForms);
        void DeleteAll(int LocalTestInstanceID);
        void UpdateAll(IEnumerable<StudentTestForms> StudentTestForms);
        List<StudentTestForms> GetStudentTestForms(int localtestformID);
        void UpdateTestFormDate(int LocalInstanceId, int ContentCategoryID, string TestDate);
        void UpdateTestFormNotes(int LocalInstanceId, int ContentCategoryID, string Notes);
        void UpdateRawScore(int? rawscore, int localtestformid, int ContentCategoryID);
        void ResetScoresSelectedIds(string ids, int LocalformInstanceId);
        List<StudentTestForms> GetStudentTestRecords(string LocalformInstanceIds);
    }
}

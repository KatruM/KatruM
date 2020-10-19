using SQLite;
using System;

namespace BDI3Mobile.Models.DBModels.DigitalTestRecord
{
    public class StudentTestForms
    {
        [PrimaryKey, AutoIncrement]
        public int LocalStudentTestFormId { get; set; }
        public int LocalformInstanceId { get; set; }
        public int contentCategoryId { get; set; }
        public int? examinerId { get; set; }
        public string testDate { get; set; }
        public int? itemScore { get; set; }
        public int? rawScore { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string Notes { get; set; }
        [Ignore]
        public string ExamierName { get; set; }
        public bool rawScoreEnabled { get; set; } = true;
        public bool BaselCeilingReached { get; set; }
        public bool IsBaselCeilingApplied { get; set; }
        public string TSOStatus { get; set; }
        public bool IsScoreSelected { get; set; }
        public int? TimeTaken { get; set; }
        public DateTime DateOfTest
        {
            get
            {
                if (!string.IsNullOrEmpty(testDate))
                {
                    var splittedDate = testDate.Split('/');
                    DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
                    return itemdateTime;
                }
                return DateTime.Now;
            }
        }
    }
}

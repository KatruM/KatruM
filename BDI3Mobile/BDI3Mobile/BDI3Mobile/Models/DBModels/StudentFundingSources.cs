using System;
using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class StudentFundingSources
    {
        [PrimaryKey]
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int FundingSourcesID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

    }
}

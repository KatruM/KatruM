using System;
using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class StudentRace
    {
        [PrimaryKey]
        public int StudentRaceID { get; set; }
        public int StudentID { get; set; }
        public int RaceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}

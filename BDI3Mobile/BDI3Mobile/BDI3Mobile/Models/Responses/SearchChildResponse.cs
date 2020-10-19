using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.Models.Responses
{
    public class SearchChildResponse
    {
        public List<SearchChildModel> data { get; set; }
        public int pageNumber { get; set; }
        public int totalPages { get; set; }
        public int totalRows { get; set; }
    }

    public class SearchChildModel
    {
        public int OfflineStudentId { get; set; }
        public string userId { get; set; }
    }

    public class ChildInfoResponse
    {
        public string childId { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string dob { get; set; }
        public string enrollmentDate { get; set; }
        public string genderText { get; set; }
        public List<ProductResearchCodes> userFields { get; set;}
        //public List<Location> location { 
        //    get; 
        //    set; }
        public string locationname
        {
            get;
            set;
        }
    }
}

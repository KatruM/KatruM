using BDI3Mobile.Models.Responses;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    interface IExaminerService
    {
        void InsertAll(List<SearchStaffResponse> examinerlist);
        List<SearchStaffResponse> GetExamainer();
        void DeleteAll();
        void DeleteByDownloadedBy(int downloadedby);
    }
}

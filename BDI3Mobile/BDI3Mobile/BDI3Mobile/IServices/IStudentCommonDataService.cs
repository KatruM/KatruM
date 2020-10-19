using BDI3Mobile.Models.Common;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IStudentCommonDataService
    {
        void InertAll(List<AllComanDataModel> comanDataModels);
        void DeleteAll();
        List<AllComanDataModel> GetStudentCommonData();
    }
}

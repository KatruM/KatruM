using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IProductResearchCodeValuesService
    {
        void InsertAll(List<ProductResearchCodeValues> productResearchCodeValues);
        List<ProductResearchCodeValues> GetProductResearchCodes(int  offlinestudenId);
        void UpdateAll(List<ProductResearchCodeValues> productResearchCodeValues);
        void InsertorUpdate(ProductResearchCodeValues productResearchCodeValues);
        void DeleteByStudentId(int offlinestudenId);
    }
}

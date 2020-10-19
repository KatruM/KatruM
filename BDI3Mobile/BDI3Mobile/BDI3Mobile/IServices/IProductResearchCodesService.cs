using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IProductResearchCodesService
    {
        void InsertAll(List<ProductResearchCodes> productResearchCodes);
        List<ProductResearchCodeValues> GetProductResearchCodes();
        void UpdateAll(List<ProductResearchCodes> productResearchCodes);
        void InsertorUpdate(ProductResearchCodes productResearchCodes);
        List<ProductResearchCodes> GetResearchCodesByOrg(int OrganizationId);
        void DeleteAll(int orgID);
    }
}

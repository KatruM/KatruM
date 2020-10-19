using System.Collections.Generic;
using BDI3Mobile.Models.Responses;

namespace BDI3Mobile.IServices
{
    public interface IOrgRecordFormService
    {
        void Insert(List<OrganizationRecordForms> organizationRecordForms);
        List<OrganizationRecordForms> GetRecordForms();
        void DeleteAll();
        void DeleteCurrentOrgRecordForm(int orgId);
    }
}

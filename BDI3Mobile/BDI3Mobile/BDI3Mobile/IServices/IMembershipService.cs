using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IMembershipService
    {
        void InsertAll(List<Membership> membershipList);
        List<Membership> GetMemberships();
        Membership GetMembershipById(int id);
        void UpdateAll(List<Membership> membershipList);
        void InsertorUpdate(Membership membership);
    }
}

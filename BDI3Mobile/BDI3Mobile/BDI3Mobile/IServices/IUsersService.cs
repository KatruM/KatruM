using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IUsersService
    {
        List<Users> GetUsers();
        Users GetUserByID(int id);
        void InsertOrUpdate(Users user);
        void InsertAll(List<Users> users);
        void UpdateAll(List<Users> users);
        Users GetUserByUserName(string UserName);
    }
}

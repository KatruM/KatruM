using System;

namespace BDI3Mobile.Models.DBModels
{
    public class UserLastConnectionActivity
    {
        public int UserId { get; set; }
        public DateTime LastConnectionDate { get; set; }
    }
}

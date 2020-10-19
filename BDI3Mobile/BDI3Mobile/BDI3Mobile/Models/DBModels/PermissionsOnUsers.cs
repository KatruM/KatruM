using System;
using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class PermissionsOnUsers
    {
        [PrimaryKey]
        public int PermissionsOnUsersID { get; set; }
        public int PermissionID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int GroupedProductId { get; set; }

    }
}

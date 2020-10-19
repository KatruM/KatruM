using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class Permissions
    {
        [PrimaryKey, AutoIncrement]
        public int LocalPermissionId { get; set; }
        public int PermissionId { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }

    }
    public class UserPermissions
    {
        [PrimaryKey, AutoIncrement]
        public int LocalUserPermissionId { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }

    }
}

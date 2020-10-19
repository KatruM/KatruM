using System;
using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class Membership
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ConfirmationToken { get; set; }
        public byte IsConfirmed { get; set; }
        public DateTime LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public string Password { get; set; }
        public DateTime PasswordChangedDate { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordVerificationToken { get; set; }
        public DateTime PasswordVerificationTokenExpirationDate { get; set; }
    }

    public class Roles
    {
        [PrimaryKey]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UsersInRoles
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int isAdjusted { get; set; }
        public byte Active { get; set; }

    }
}

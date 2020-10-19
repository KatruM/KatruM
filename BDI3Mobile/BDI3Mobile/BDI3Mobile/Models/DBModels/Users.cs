using System;
using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class Users
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public string DeviceId { get; set; }
        public int UserTypeID { get; set; }
        [Unique]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int StatusID { get; set; }
        public int CountryID { get; set; }
        public byte IsPasswordActive { get; set; }
        public string MiddleName { get; set; }
        public int GenderID { get; set; }
        public string State { get; set; }
        public int OrganizationID { get; set; }
        public int DeleteTypeID { get; set; }
        public string U_Username { get; set; }
        public string UpperUsername { get; set; }
        public byte HelpStatus { get; set; }
        public string StudentImportRelatedId { get; set; }
        public int isDebug { get; set; }
        public int AddedBy { get; set; }
        public byte IsNewPasswordActive { get; set; }
        public int PlatformID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class UserTypes
    {
        [PrimaryKey]
        public int UserTypeID { get; set; }
        public string UserTypeName { get; set; }
    }
}


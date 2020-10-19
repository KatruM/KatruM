using Newtonsoft.Json;
namespace BDI3Mobile.Helpers
{
    public class APIResponse
    {
        public APIResponse()
        {

        }
        public int message_code { get; set; }
        public string Message { get; set; }
    }


    public class LoginResponse
    {
        public LoginResponse() { }
        public string token { get; set; }
        public string StatusCode { get; set; }

    }

    public class TokenResponse
    {
        [JsonProperty("uid")]
        public string UserID { get; set; }
        [JsonProperty("orgid")]
        public string OrganizationID { get; set; }
        [JsonProperty("un")]
        public string UserName { get; set; }
        [JsonProperty("rid")]
        public string RoleID { get; set; }
        [JsonProperty("rn")]
        public string RoleName { get; set; }
        public string utid { get; set; }
        [JsonProperty("fn")]
        public string FirstName { get; set; }
        [JsonProperty("ln")]
        public string LastName { get; set; }
        [JsonProperty("ph")]
        public string Phone { get; set; }
        [JsonProperty("eml")]
        public string Email { get; set; }
        [JsonProperty("userRoles")]
        public string UserRoles { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
        [JsonProperty("perms")]
        public string Perms { get; set; }

        public string Token { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string UserName { get; set; }
    }
}

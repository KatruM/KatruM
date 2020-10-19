namespace BDI3Mobile.Services
{
    using BDI3Mobile.Helpers;
    using BDI3Mobile.IServices;

    public class TokenService : ITokenService
    {
        TokenResponse TokenResponse;
        public TokenService()
        {
            TokenResponse = new TokenResponse();
        }
        public void ClearTokenResponse()
        {
            TokenResponse = new TokenResponse();
        }

        public TokenResponse GetTokenResposne()
        {
            return TokenResponse;
        }

        public void SetTokenResponse(TokenResponse tokenResponse)
        {
            TokenResponse = tokenResponse;
        }
    }
}

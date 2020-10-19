using BDI3Mobile.Helpers;

namespace BDI3Mobile.IServices
{
    public interface ITokenService
    {
        void SetTokenResponse(TokenResponse tokenResponse);
        TokenResponse GetTokenResposne();
        void ClearTokenResponse();
    }
}

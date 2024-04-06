using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Authentication.Input;
using BookeryMobile.Data.DTOs.Authentication.Output;
using BookeryMobile.Exceptions;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.Authentication
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        public AuthenticationService() : base("Authentication")
        {
            
        }

        public async Task<TokenDto?> GetToken(GetTokenDto getTokenDto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("Token", getTokenDto)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TokenDto>();
                }
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    throw new ServiceUnavailableException();
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new InvalidCredentialException();
                }

                return null;
            }
            catch (WebException e)
            {
                throw new ServiceUnavailableException();
            }
        }

        public async Task<TokenDto?> RefreshToken(string accessToken, string refreshToken)
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var refreshTokenRequest = new RefreshTokenDto(refreshToken);

            try
            {
                var response = await HttpClient.PostAsJsonAsync("Token/Refresh", refreshTokenRequest);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TokenDto>();
                }

                return null;
            }
            catch (WebException e)
            {
                throw new ServiceUnavailableException();
            }
        }

        public async Task SignOut()
        {
            try
            {
                await HttpClient.DeleteAsync("SignOut");
            }
            catch (WebException e)
            {
                throw new ServiceUnavailableException();
            }
        }
    }
}
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Authentication.Input;
using BookeryMobile.Data.DTOs.Authentication.Output;

namespace BookeryMobile.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<TokenDto?> GetToken(GetTokenDto getTokenDto);
        Task<TokenDto?> RefreshToken(string accessToken, string refreshToken);
        Task SignOut();
    }
}
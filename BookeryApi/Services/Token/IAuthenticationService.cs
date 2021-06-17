using System.Threading.Tasks;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;

namespace BookeryApi.Services.Token
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> GetToken(AuthenticationRequest authenticationRequest);
        Task<AuthenticationResponse> RefreshToken(string accessToken, string refreshToken);
        Task<SignUpResult> SignUp(string email, string username, string password);
        Task LogOut();
    }
}
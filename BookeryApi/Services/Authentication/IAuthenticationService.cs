using System.Threading.Tasks;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;

namespace BookeryApi.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> GetToken(AuthenticationRequest authenticationRequest);
        Task<AuthenticationResponse> RefreshToken(string accessToken, string refreshToken);
        Task<SignUpResult> SignUp(SignUpRequest signUpRequest);
        Task LogOut();
    }
}
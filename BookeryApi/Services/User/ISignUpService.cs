using System.Threading.Tasks;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;

namespace BookeryApi.Services.User
{
    public interface ISignUpService
    {
        Task<SignUpResult> SignUp(SignUpRequest signUpRequest);
    }
}
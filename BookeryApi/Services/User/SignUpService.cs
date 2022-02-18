using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryApi.Exceptions;
using BookeryApi.Services.Common;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;

namespace BookeryApi.Services.User
{
    public class SignUpService : BaseService, ISignUpService
    {
        public SignUpService() : base("SignUp")
        {
            
        }
        
        public async Task<SignUpResult> SignUp(SignUpRequest signUpRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("", signUpRequest);

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
            }

            return await response.Content.ReadAsAsync<SignUpResult>();
        }
    }
}
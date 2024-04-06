using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.User.Input;
using BookeryMobile.Data.DTOs.User.Output;
using BookeryMobile.Exceptions;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.User
{
    public class UserService : BaseService, IUserService
    {
        public UserService() : base("User")
        {
        }

        public async Task SignUp(UserSignUpDto userSignUpDto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("SignUp", userSignUpDto);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return;
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new UserAlreadyExistsException(userSignUpDto.Email);
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new InvalidEmailException(userSignUpDto.Email);
                }

                throw new ServiceUnavailableException();
            }
            catch (WebException e)
            {
                throw new ServiceUnavailableException();
            }
        }

        public async Task<UserDto> Get()
        {
            var response = await HttpClient.GetAsync("Self");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<UserDto>();
            }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
            }

            throw new UserNotFoundException();
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var response = await HttpClient.GetAsync(Path.Combine("ByEmail", email));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<UserDto>();
            }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
            }

            throw new UserNotFoundException();
        }

        public async Task<UserDto> GetById(Guid id)
        {
            var response = await HttpClient.GetAsync(Path.Combine("ById", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<UserDto>();
            }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
            }

            throw new UserNotFoundException();
        }
    }
}
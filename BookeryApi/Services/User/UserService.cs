using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookeryApi.Exceptions;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.User
{
    public class UserService : BaseService, IUserService
    {
        public UserService() : base("User")
        {
            
        }

        public async Task<Domain.Models.User> Get()
        {
            var response = await _httpClient.GetAsync("self");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Domain.Models.User>();
            }
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
            }

            throw new DataNotFoundException("User");
        }

        public async Task<Domain.Models.User> GetByEmail(string email)
        {
            var response = await _httpClient.GetAsync(Path.Combine("email", email));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Domain.Models.User>();
            }
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
            }

            throw new DataNotFoundException("User");
        }

        public async Task<Domain.Models.User> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync(Path.Combine("id", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Domain.Models.User>();
            }
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
            }

            throw new DataNotFoundException("User");
        }
    }
}
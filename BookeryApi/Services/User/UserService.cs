using System;
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
            var response = await _httpClient.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Domain.Models.User>();
            }

            throw new DataNotFoundException("User");
        }

        public async Task<Domain.Models.User> GetByEmail(string email)
        {
            var response = await _httpClient.GetAsync(email);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Domain.Models.User>();
            }

            throw new DataNotFoundException("User");
        }
    }
}
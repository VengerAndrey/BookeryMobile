using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookeryApi.Exceptions;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;

namespace BookeryApi.Services.Token
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:42396/api/Authentication/");
        }

        public async Task<AuthenticationResponse> GetToken(AuthenticationRequest authenticationRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("token", authenticationRequest)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<AuthenticationResponse>();
            }

            throw new InvalidCredentialException();
        }

        public async Task<AuthenticationResponse> RefreshToken(string accessToken, string refreshToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = refreshToken
            };

            var response = await _httpClient.PostAsJsonAsync("refresh-token", refreshTokenRequest);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<AuthenticationResponse>();
            }

            return null;
        }

        public async Task<SignUpResult> SignUp(string email, string username, string password)
        {
            var signUpRequest = new SignUpRequest
            {
                Email = email,
                Username = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("sign-up", signUpRequest);

            return await response.Content.ReadAsAsync<SignUpResult>();
        }

        public async Task LogOut()
        {
            await _httpClient.PostAsync("log-out", null);
        }
    }
}
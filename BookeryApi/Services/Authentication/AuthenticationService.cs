﻿using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookeryApi.Exceptions;
using BookeryApi.Services.Common;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;

namespace BookeryApi.Services.Authentication
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        public AuthenticationService() : base("Authentication")
        {
            
        }

        public async Task<AuthenticationResponse> GetToken(AuthenticationRequest authenticationRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("token", authenticationRequest)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<AuthenticationResponse>();
            }
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException();
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

        public async Task LogOut()
        {
            await _httpClient.DeleteAsync("sign-out");
        }
    }
}
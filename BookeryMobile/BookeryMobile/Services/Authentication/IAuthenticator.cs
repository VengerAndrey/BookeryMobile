using System;
using System.Threading.Tasks;
using Domain.Models.DTOs.Responses;

namespace BookeryMobile.Services.Authentication
{
    internal interface IAuthenticator
    {
        bool IsSignedIn { get; }

        event Action StateChanged;

        Task SignIn(string email, string password);

        void SignOut();
    }
}
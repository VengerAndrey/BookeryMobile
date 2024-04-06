using System;
using System.Threading.Tasks;

namespace BookeryMobile.Services.Authenticator
{
    internal interface IAuthenticator
    {
        bool IsSignedIn { get; }

        event Action StateChanged;

        Task SignIn(string email, string password);

        void SignOut();
    }
}
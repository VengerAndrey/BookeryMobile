using System;
using System.Threading;
using System.Threading.Tasks;
using BookeryApi.Services.Authentication;
using BookeryApi.Services.Node;
using BookeryApi.Services.Storage;
using BookeryApi.Services.User;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;
using Xamarin.Forms;

namespace BookeryMobile.Services.Authentication
{
    internal class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService
            _authenticationService = DependencyService.Get<IAuthenticationService>();
        private readonly IPrivateNodeService _privateNodeService = DependencyService.Get<IPrivateNodeService>();
        private readonly ISharedNodeService _sharedNodeService = DependencyService.Get<ISharedNodeService>();
        private readonly IStorageService _storageService = DependencyService.Get<IStorageService>();
        private readonly IUserService _userService = DependencyService.Get<IUserService>();

        private AuthenticationResponse _currentAuthenticationResponse;

        private Timer _timer;

        public bool IsSignedIn => _currentAuthenticationResponse != null;
        public event Action StateChanged;

        public async Task SignIn(string email, string password)
        {
            await Authenticate(new AuthenticationRequest {Email = email, Password = password});
            _timer = new Timer(async o => { await RefreshToken(); }, null, TimeSpan.Zero,
                _currentAuthenticationResponse.ExpireAt - DateTime.UtcNow);
            StateChanged?.Invoke();
        }

        public async Task<SignUpResult> SignUp(string email, string lastName, string firstName, string password)
        {
            return await _authenticationService.SignUp(new SignUpRequest()
            {
                Email = email,
                LastName = lastName,
                FirstName = firstName,
                Password = password
            });
        }

        public void SignOut()
        {
            _authenticationService.LogOut();
            _timer?.Change(Timeout.Infinite, 0);
            _currentAuthenticationResponse = null;
            StateChanged?.Invoke();
        }

        private async Task Authenticate(AuthenticationRequest authenticationRequest)
        {
            _currentAuthenticationResponse = await _authenticationService.GetToken(authenticationRequest);
            SetBearerTokenToServices();
        }

        private async Task RefreshToken()
        {
            _currentAuthenticationResponse = await _authenticationService
                .RefreshToken(_currentAuthenticationResponse.AccessToken, _currentAuthenticationResponse.RefreshToken);
            SetBearerTokenToServices();
        }

        private void SetBearerTokenToServices()
        {
            if (_currentAuthenticationResponse == null)
            {
                return;
            }
            _privateNodeService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _storageService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _userService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _sharedNodeService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
        }
    }
}
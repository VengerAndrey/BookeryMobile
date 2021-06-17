using System;
using System.Threading;
using System.Threading.Tasks;
using BookeryApi.Services.Storage;
using BookeryApi.Services.Token;
using BookeryApi.Services.User;
using Domain.Models.DTOs.Requests;
using Domain.Models.DTOs.Responses;
using Xamarin.Forms;

namespace BookeryMobile.Services.Authentication
{
    internal class Authenticator : IAuthenticator
    {
        private readonly IAccessService _accessService = DependencyService.Get<IAccessService>();

        private readonly IAuthenticationService
            _authenticationService = DependencyService.Get<IAuthenticationService>();

        private readonly IItemService _itemService = DependencyService.Get<IItemService>();
        private readonly IPhotoService _photoService = DependencyService.Get<IPhotoService>();
        private readonly IShareService _shareService = DependencyService.Get<IShareService>();
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

        public async Task<SignUpResult> SignUp(string email, string username, string password)
        {
            return await _authenticationService.SignUp(email, username, password);
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
            _shareService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _itemService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _shareService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _userService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _accessService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
            _photoService.SetBearerToken(_currentAuthenticationResponse.AccessToken);
        }
    }
}
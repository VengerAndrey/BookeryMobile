using System;
using System.Threading;
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Authentication.Input;
using BookeryMobile.Data.DTOs.Authentication.Output;
using BookeryMobile.Services.Authentication;
using BookeryMobile.Services.Node.Interfaces;
using BookeryMobile.Services.Photo;
using BookeryMobile.Services.Storage;
using BookeryMobile.Services.User;
using Xamarin.Forms;

namespace BookeryMobile.Services.Authenticator
{
    internal class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService
            _authenticationService = DependencyService.Get<IAuthenticationService>();

        private readonly IPrivateNodeService _privateNodeService = DependencyService.Get<IPrivateNodeService>();
        private readonly ISharedNodeService _sharedNodeService = DependencyService.Get<ISharedNodeService>();
        private readonly IStorageService _storageService = DependencyService.Get<IStorageService>();
        private readonly IUserService _userService = DependencyService.Get<IUserService>();
        private readonly IPhotoService _photoService = DependencyService.Get<IPhotoService>();
        private readonly ISharingService _sharingService = DependencyService.Get<ISharingService>();

        private TokenDto? _tokenDto;

        private Timer? _timer;

        public bool IsSignedIn => _tokenDto != null;
        public event Action? StateChanged;

        public async Task SignIn(string email, string password)
        {
            _tokenDto = await _authenticationService.GetToken(new GetTokenDto(email, password));
            if (_tokenDto == null)
            {
                return;
            }

            SetBearerTokenToServices();
            _timer = new Timer(async o => { await RefreshToken(); }, null, TimeSpan.Zero,
                _tokenDto.ExpireAt - DateTime.UtcNow);
            StateChanged?.Invoke();
        }

        public void SignOut()
        {
            _authenticationService.SignOut();
            _timer?.Change(Timeout.Infinite, 0);
            _tokenDto = null;
            StateChanged?.Invoke();
        }

        private async Task RefreshToken()
        {
            if (_tokenDto == null)
            {
                return;
            }

            _tokenDto = await _authenticationService
                .RefreshToken(_tokenDto.AccessToken, _tokenDto.RefreshToken);
            SetBearerTokenToServices();
        }

        private void SetBearerTokenToServices()
        {
            if (_tokenDto == null)
            {
                return;
            }

            _privateNodeService.SetBearerToken(_tokenDto.AccessToken);
            _storageService.SetBearerToken(_tokenDto.AccessToken);
            _userService.SetBearerToken(_tokenDto.AccessToken);
            _sharedNodeService.SetBearerToken(_tokenDto.AccessToken);
            _photoService.SetBearerToken(_tokenDto.AccessToken);
            _sharingService.SetBearerToken(_tokenDto.AccessToken);
        }
    }
}
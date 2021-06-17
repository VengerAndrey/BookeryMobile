using BookeryApi.Exceptions;
using BookeryMobile.Services.Authentication;
using BookeryMobile.Views;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IAuthenticator _authenticator = DependencyService.Get<IAuthenticator>();
        private string _email = "email@gmail.com";
        private string _password = "123";

        public SignInViewModel()
        {
            SignInCommand = new Command(OnSignIn, CanSignIn);
            _authenticator.StateChanged += async () =>
            {
                if (_authenticator.IsSignedIn)
                {
                    await Shell.Current.GoToAsync($"//{nameof(SharesPage)}");
                }
            };
        }

        public Command SignInCommand { get; }

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                SignInCommand.ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                SignInCommand.ChangeCanExecute();
            }
        }

        public void OnAppearing()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        private async void OnSignIn()
        {
            try
            {
                await _authenticator.SignIn(Email, Password);
            }
            catch (InvalidCredentialException e)
            {
                Email = "WRONG";
            }
        }

        private bool CanSignIn()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }
    }
}
using BookeryMobile.Common;
using BookeryMobile.Exceptions;
using BookeryMobile.Services.Authenticator;
using BookeryMobile.Views;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IAuthenticator _authenticator = DependencyService.Get<IAuthenticator>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();

        private string _email = "";
        private string _password = "";

        public SignInViewModel()
        {
            SignInCommand = new Command(OnSignIn, CanSignIn);
            ViewSignUpCommand = new Command(ViewSignUp);
            _authenticator.StateChanged += async () =>
            {
                if (_authenticator.IsSignedIn)
                {
                    await Shell.Current.GoToAsync($"//{nameof(PrivateNodesPage)}");
                }
                else
                {
                    Application.Current.MainPage = new AppShell();
                }
            };
        }

        public Command SignInCommand { get; }
        public Command ViewSignUpCommand { get; }

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
            catch (ServiceUnavailableException e)
            {
                _message.Short(e.Message);
            }
            catch (InvalidCredentialException e)
            {
                _message.Short(e.Message);
            }
        }

        private bool CanSignIn()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }

        private async void ViewSignUp()
        {
            await Shell.Current.GoToAsync($"//{nameof(SignUpPage)}");
        }
    }
}
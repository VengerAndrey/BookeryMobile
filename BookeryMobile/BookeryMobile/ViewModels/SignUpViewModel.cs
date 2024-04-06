using BookeryMobile.Common;
using BookeryMobile.Data.DTOs.User.Input;
using BookeryMobile.Exceptions;
using BookeryMobile.Services.User;
using BookeryMobile.Views;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class SignUpViewModel : BaseViewModel
    {
        private readonly IUserService _signUpService = DependencyService.Get<IUserService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private string _confirmPassword = "";

        private string _email = "";
        private string _password = "";
        private string _firstName = "";
        private string _lastName = "";

        public SignUpViewModel()
        {
            SignUpCommand = new Command(SignUp, CanSignUp);
            ViewSignInCommand = new Command(ViewSignIn);
        }

        public Command SignUpCommand { get; }
        public Command ViewSignInCommand { get; }

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                SignUpCommand.ChangeCanExecute();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                SignUpCommand.ChangeCanExecute();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                SignUpCommand.ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                SignUpCommand.ChangeCanExecute();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                SignUpCommand.ChangeCanExecute();
            }
        }

        public void OnAppearing()
        {
            Email = string.Empty;
            FirstName = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private async void SignUp()
        {
            var userSignUpDto = new UserSignUpDto(Email, Password, FirstName, LastName);
            try
            {
                await _signUpService.SignUp(userSignUpDto);
                await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
            }
            catch (UserAlreadyExistsException)
            {
                _message.Short("Email is already is use.");
            }
            catch (InvalidEmailException)
            {
                _message.Short("Email is invalid.");
            }
            catch (ServiceUnavailableException e)
            {
                _message.Short(e.Message);
            }
        }

        private bool CanSignUp()
        {
            return !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(ConfirmPassword) &&
                   Password == ConfirmPassword;
        }

        private async void ViewSignIn()
        {
            await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
        }
    }
}
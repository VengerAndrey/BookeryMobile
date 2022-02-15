using System;
using BookeryMobile.Common;
using BookeryMobile.Services.Authentication;
using BookeryMobile.Views;
using Domain.Models.DTOs.Responses;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class SignUpViewModel : BaseViewModel
    {
        private readonly IAuthenticator _authenticator = DependencyService.Get<IAuthenticator>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private string _confirmPassword = "123";

        private string _email = "mobile@gmail.com";
        private string _password = "123";
        private string _firstName = "first";
        private string _lastName = "second";

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
            var signUpResult = await _authenticator.SignUp(Email, LastName, FirstName, Password);

            switch (signUpResult)
            {
                default:
                case SignUpResult.Success:
                    await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
                    break;
                case SignUpResult.EmailAlreadyExists:
                    _message.Short("Email already exists.");
                    break;
                case SignUpResult.InvalidEmail:
                    _message.Short("Email is invalid.");
                    break;
            }
        }

        private bool CanSignUp()
        {
            return !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(FirstName) &&
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
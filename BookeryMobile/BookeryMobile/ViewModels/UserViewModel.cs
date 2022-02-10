using System;
using System.IO;
using System.Windows.Input;
using BookeryApi.Services.User;
using BookeryMobile.Services.Authentication;
using BookeryMobile.Views;
using Domain.Models;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private readonly IAuthenticator _authenticator = DependencyService.Get<IAuthenticator>();
        // private readonly IPhotoService _photoService = DependencyService.Get<IPhotoService>();
        private readonly IUserService _userService = DependencyService.Get<IUserService>();
        private ImageSource _profileImageSource;

        private User _user;

        public UserViewModel()
        {
            Title = "Profile";
            LoadUserCommand = new Command(LoadUser);
            LoadProfileImageCommand = new Command(LoadProfilePhoto);
            LogOutCommand = new Command(async () =>
            {
                _authenticator.SignOut();
                await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
            });
        }

        public ICommand LoadUserCommand { get; }
        public ICommand LoadProfileImageCommand { get; }
        public ICommand LogOutCommand { get; }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set => SetProperty(ref _profileImageSource, value);
        }

        public void OnAppearing()
        {
            LoadUserCommand.Execute(null);
            LoadProfileImageCommand.Execute(null);
        }

        private async void LoadUser()
        {
            try
            {
                User = await _userService.Get();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async void LoadProfilePhoto()
        {
            // try
            // {
            //     var content = await _photoService.Get();
            //
            //     if (content != null)
            //     {
            //         byte[] bytes;
            //         using (var memoryStream = new MemoryStream())
            //         {
            //             await content.CopyToAsync(memoryStream);
            //             bytes = memoryStream.ToArray();
            //         }
            //
            //         ProfileImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
            //     }
            // }
            // catch (Exception e)
            // {
            // }
        }
    }
}
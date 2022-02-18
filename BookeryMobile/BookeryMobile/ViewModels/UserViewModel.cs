using System;
using System.IO;
using System.Windows.Input;
using BookeryApi.Exceptions;
using BookeryApi.Services.Photo;
using BookeryApi.Services.User;
using BookeryMobile.Common;
using BookeryMobile.Services.Authentication;
using BookeryMobile.Views;
using Domain.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private readonly IAuthenticator _authenticator = DependencyService.Get<IAuthenticator>();
        private readonly IUserService _userService = DependencyService.Get<IUserService>();
        private readonly IPhotoService _photoService = DependencyService.Get<IPhotoService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private ImageSource _profileImageSource;
        private PopupPage _page;

        private User _user;

        public UserViewModel()
        {
            Title = "Profile";
            LoadUserCommand = new Command(LoadUser);
            LoadProfileImageCommand = new Command(LoadProfilePhoto);
            UploadProfileImageCommand = new Command(UploadProfilePhoto, () => User != null);
            LogOutCommand = new Command(async () =>
            {
                _authenticator.SignOut();
                await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
            });
        }

        public ICommand LoadUserCommand { get; }
        public ICommand LoadProfileImageCommand { get; }
        public Command UploadProfileImageCommand { get; }
        public ICommand LogOutCommand { get; }

        public User User
        {
            get => _user;
            set
            {
                SetProperty(ref _user, value);
                UploadProfileImageCommand.ChangeCanExecute();
            }
        }

        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set => SetProperty(ref _profileImageSource, value);
        }

        public void OnAppearing()
        {
            LoadUserCommand.Execute(null);
        }

        private async void LoadUser()
        {
            try
            {
                User = await _userService.Get();
                LoadProfileImageCommand.Execute(null);
            }
            catch (DataNotFoundException e)
            {
                _message.Short(e.Message);
            }
            catch (ServiceUnavailableException e)
            {
                _message.Short(e.Message);
            }
        }

        private async void LoadProfilePhoto()
        {
            try
            {
                var content = await _photoService.DownloadProfilePhoto();
            
                if (content != null)
                {
                    byte[] bytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await content.CopyToAsync(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
            
                    ProfileImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private async void UploadProfilePhoto()
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select photo",
                    FileTypes = FilePickerFileType.Images
                });
                if (file != null)
                {
                    PushPopupPage(new LoadingPage());
            
                    var stream = await file.OpenReadAsync();
                    var result = await _photoService.UploadProfilePhoto(stream);
            
                    if (result)
                    {
                        LoadProfileImageCommand.Execute(null);
                    }
                    else
                    {
                        _message.Short("Unable to upload profile photo.");
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                PopPopupPage();
            }
        }
        
        private async void PushPopupPage(PopupPage page)
        {
            _page = page;
            await PopupNavigation.Instance.PushAsync(_page);
        }

        private async void PopPopupPage()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BookeryMobile.Common;
using BookeryMobile.Data.DTOs.Node.Input;
using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.Data.Enums;
using BookeryMobile.Exceptions;
using BookeryMobile.Services.Node.Interfaces;
using BookeryMobile.Services.User;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;
using UserDto = BookeryMobile.Data.DTOs.User.Output.UserDto;

namespace BookeryMobile.ViewModels
{
    public class ShareNodeViewModel : BaseViewModel
    {
        private readonly ISharingService _sharingService = DependencyService.Get<ISharingService>();
        private readonly IUserService _userService = DependencyService.Get<IUserService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;
        private string _userEmail = "";
        private bool _isWriteAccess;

        public ShareNodeViewModel(IPopupNavigation popupNavigation, NodeDto node)
        {
            _popupNavigation = popupNavigation;
            Title = "Share item";
            NodeDto = node;
            Sharing = new ObservableCollection<UserDto>();
            SubmitCommand = new Command(ShareNode, CanShareNode);
            HideNodeCommand = new Command<UserDto>(HideNode);
            
        }

        private NodeDto NodeDto { get; set; }

        public string UserEmail
        {
            get => _userEmail;
            set
            {
                SetProperty(ref _userEmail, value);
                SubmitCommand.ChangeCanExecute();
            }
        }

        public bool IsWriteAccess
        {
            get => _isWriteAccess;
            set => SetProperty(ref _isWriteAccess, value);
        }

        public ObservableCollection<UserDto> Sharing { get; }

        public Command SubmitCommand { get; }
        public Command<UserDto> HideNodeCommand { get; }

        public Task OnAppearing() => LoadSharing();

        private async void ShareNode()
        {
            try
            {
                var owner = await _userService.Get();
                var user = await _userService.GetByEmail(UserEmail);

                if (owner is null || user is null)
                {
                    throw new UserNotFoundException();
                }

                var result = await _sharingService.Share(new ShareNodeDto(NodeDto.Id, user.Id,
                    IsWriteAccess ? AccessTypeId.Write : AccessTypeId.Read));

                if (result)
                {
                    _message.Short("Shared successfully.");
                }
            }
            catch (UserNotFoundException e)
            {
                _message.Short(e.Message);
            }
            finally
            {
                await _popupNavigation.PopAsync();
            }
        }

        private bool CanShareNode()
        {
            return !string.IsNullOrEmpty(UserEmail);
        }

        private async Task LoadSharing()
        {
            var sharing = await _sharingService.GetSharing(NodeDto.Id);
            if (sharing != null)
            {
                foreach (var user in sharing)
                {
                    Sharing.Add(new UserDto(user.Id, user.Email, user.FirstName, user.LastName));
                }
            }
        }

        private async void HideNode(UserDto user)
        {
            var result = await _sharingService.Hide(new HideNodeDto(NodeDto.Id, user.Id));

            if (result)
            {
                _message.Short("Hidden successfully.");
            }

            await _popupNavigation.PopAsync();
        }
    }
}
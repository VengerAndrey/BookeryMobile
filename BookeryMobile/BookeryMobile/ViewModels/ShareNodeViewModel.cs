using BookeryApi.Exceptions;
using BookeryApi.Services.Node;
using BookeryApi.Services.User;
using BookeryMobile.Common;
using Domain.Models;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class ShareNodeViewModel: BaseViewModel
    {
        private readonly ISharedNodeService _sharedNodeService = DependencyService.Get<ISharedNodeService>();
        private readonly IUserService _userService = DependencyService.Get<IUserService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;
        private string _userEmail = "client@gmail.com";
        private bool _isWriteAccess;
        
        public ShareNodeViewModel(IPopupNavigation popupNavigation, Node node)
        {
            _popupNavigation = popupNavigation;
            Title = "Share item";
            Node = node;
            SubmitCommand = new Command(ShareNode, CanShareNode);
        }
        
        public Node Node { get; set; }
        
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
        
        public Command SubmitCommand { get; }
        
        private async void ShareNode()
        {
            try
            {
                var owner = await _userService.Get();
                var user = await _userService.GetByEmail(UserEmail);

                if (owner is null || user is null)
                {
                    throw new DataNotFoundException("User");
                }

                var result = await _sharedNodeService.Share(new UserNode
                {
                    NodeId = Node.Id,
                    UserId = user.Id,
                    AccessTypeId = IsWriteAccess ? AccessTypeId.Write : AccessTypeId.Read
                });
                
                if (!result)
                {
                    throw new ForbiddenException();
                }
            }
            catch (ForbiddenException e)
            {
                _message.Short(e.Message);
            }
            catch (DataNotFoundException e)
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
    }
}
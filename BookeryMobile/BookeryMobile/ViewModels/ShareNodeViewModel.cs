using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ISharingService _sharingService = DependencyService.Get<ISharingService>();
        private readonly IUserService _userService = DependencyService.Get<IUserService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;
        private string _userEmail = "";
        private bool _isWriteAccess;
        
        public ShareNodeViewModel(IPopupNavigation popupNavigation, Node node)
        {
            _popupNavigation = popupNavigation;
            Title = "Share item";
            Node = node;
            Sharing = new ObservableCollection<User>();
            SubmitCommand = new Command(ShareNode, CanShareNode);
            HideNodeCommand = new Command<User>(HideNode);
            LoadSharing();
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
        
        public ObservableCollection<User> Sharing { get; }

        public Command SubmitCommand { get; }
        public Command<User> HideNodeCommand { get; }
        
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

                var result = await _sharingService.Share(new UserNode
                {
                    NodeId = Node.Id,
                    UserId = user.Id,
                    AccessTypeId = IsWriteAccess ? AccessTypeId.Write : AccessTypeId.Read
                });

                if (result)
                {
                    _message.Short("Shared successfully.");
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

        private async Task LoadSharing()
        {
            var sharing = await _sharingService.GetSharing(Node.Id);
            foreach (var userNode in sharing)
            {
                Sharing.Add(await _userService.GetById(userNode.UserId));
            }
        }

        private async void HideNode(User user)
        {
            var result = await _sharingService.Hide(new UserNode
            {
                NodeId = Node.Id,
                UserId = user.Id
            });
            
            if (result)
            {
                _message.Short("Hidden successfully.");
            }
            
            await _popupNavigation.PopAsync();
        }
    }
}
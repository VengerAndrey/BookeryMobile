using BookeryApi.Exceptions;
using BookeryApi.Services.Node;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using Domain.Models;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class RenameNodeViewModel : BaseViewModel
    {
        private readonly string _extension;
        private readonly INodeUpdateService _nodeService;
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;
        private readonly string _path;
        
        public RenameNodeViewModel(IPopupNavigation popupNavigation, INodeUpdateService nodeService, string path, Node node)
        {
            _popupNavigation = popupNavigation;
            _nodeService = nodeService;
            Title = "Rename";
            _path = path.Trim('/');
            Node = node;
            _extension = node.IsDirectory || !node.Name.Contains(".") ? "" : node.Name.Substring(node.Name.LastIndexOf('.'));
            SubmitCommand = new Command(RenameItem, CanRenameItem);
        }
        
        public Node Node { get; set; }
        
        public string Name
        {
            get => Node.IsDirectory || !Node.Name.Contains(".") ? Node.Name : Node.Name.Substring(0, Node.Name.LastIndexOf('.'));
            set
            {
                Node.Name = value + _extension;
                OnPropertyChanged(nameof(Name));
                SubmitCommand.ChangeCanExecute();
            }
        }
        
        public Command SubmitCommand { get; }
        
        private async void RenameItem()
        {
            try
            {
                await _nodeService.Update(_path, Node);
            }
            catch (ForbiddenException e)
            {
                _message.Short(e.Message);
            }
            catch (WrongAccessTypeException e)
            {
                _message.Short(e.Message);
            }
            finally
            {
                await _popupNavigation.PopAsync();
            }
        }
        
        private bool CanRenameItem()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
using BookeryApi.Services.Node;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using Domain.Models;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class CreateDirectoryViewModel : BaseViewModel
    {
        private readonly INodeCreateService _nodeService;
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;
        private string _name;
        private readonly string _path;

        public CreateDirectoryViewModel(IPopupNavigation popupNavigation, INodeCreateService nodeService, string path)
        {
            _popupNavigation = popupNavigation;
            _nodeService = nodeService;
            Title = "Create folder";
            _path = path;
            SubmitCommand = new Command(CreateDirectory, CanCreateDirectory);
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                SubmitCommand.ChangeCanExecute();
            }
        }

        public Command SubmitCommand { get; }

        private async void CreateDirectory()
        {
            var result = await _nodeService.Create(_path, new Node()
            {
                IsDirectory = true,
                Name = Name
            });

            if (result is null)
            {
                _message.Short("Folder with the same name already exists.");
            }

            await _popupNavigation.PopAsync();
        }

        private bool CanCreateDirectory()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
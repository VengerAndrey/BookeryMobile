using BookeryMobile.Common;
using BookeryMobile.Data.DTOs.Node.Input;
using BookeryMobile.Exceptions;
using BookeryMobile.Services.Node.Interfaces;
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
            Title = "Create";
            _path = path;
            _name = string.Empty;
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
            try
            {
                await _nodeService.Create(_path, new CreateNodeDto(Name, true));
            }
            catch (NameConflictException e)
            {
                _message.Short(e.Message);
            }

            await _popupNavigation.PopAsync();
        }

        private bool CanCreateDirectory()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
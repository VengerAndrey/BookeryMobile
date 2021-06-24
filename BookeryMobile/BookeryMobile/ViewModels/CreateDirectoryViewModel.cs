using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using Domain.Models;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class CreateDirectoryViewModel : BaseViewModel
    {
        private readonly IItemService _itemService = DependencyService.Get<IItemService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;
        private string _name;

        public CreateDirectoryViewModel(IPopupNavigation popupNavigation, Item item)
        {
            _popupNavigation = popupNavigation;
            Title = "Create folder";
            Item = item;
            SubmitCommand = new Command(CreateDirectory, CanCreateDirectory);
        }

        public Item Item { get; set; }

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
            var result = await _itemService.CreateDirectory($"{Item.Path}/{_name}");

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
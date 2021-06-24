using BookeryApi.Exceptions;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using Domain.Models;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class RenameItemViewModel : BaseViewModel
    {
        private readonly string _extension;
        private readonly IItemService _itemService = DependencyService.Get<IItemService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;

        public RenameItemViewModel(IPopupNavigation popupNavigation, Item item)
        {
            _popupNavigation = popupNavigation;
            Title = "Rename item";
            Item = item;
            _extension = item.IsDirectory ? "" : item.Name.Substring(item.Name.LastIndexOf('.'));
            SubmitCommand = new Command(RenameItem, CanRenameItem);
        }

        public Item Item { get; set; }

        public string Name
        {
            get => Item.IsDirectory ? Item.Name : Item.Name.Substring(0, Item.Name.LastIndexOf('.'));
            set
            {
                Item.Name = value + _extension;
                OnPropertyChanged(nameof(Name));
                SubmitCommand.ChangeCanExecute();
            }
        }

        public Command SubmitCommand { get; }

        private async void RenameItem()
        {
            try
            {
                if (!Item.IsDirectory)
                {
                    await _itemService.RenameFile(Item.Path, Item.Name);
                }
                else
                {
                    _message.Short("Folder renaming is currently not available.");
                }
            }
            catch (ForbiddenException e)
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
using System.Collections.ObjectModel;
using System.IO;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using BookeryMobile.Models;
using BookeryMobile.Views;
using Domain.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class ItemsViewModel : BaseViewModel
    {
        private readonly IItemService _itemService = DependencyService.Get<IItemService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly INavigation _navigation;
        private readonly Item _item;
        private PopupPage _page;

        public ItemsViewModel(INavigation navigation, Item item)
        {
            _navigation = navigation;
            _item = item;
            Title = item.Name;
            Items = new ObservableCollection<ItemElement>();

            LoadItemsCommand = new Command(LoadItems);
            SelectItemCommand = new Command<Item>(SelectItem);
            DeleteItemCommand = new Command<Item>(DeleteItem);
            RenameItemCommand = new Command<Item>(OpenRenameItemPopup);

            PopupNavigation.Instance.Popping += (sender, args) =>
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0 && args.Page == _page)
                {
                    OnAppearing();
                }
            };
        }

        public ObservableCollection<ItemElement> Items { get; }

        public Command LoadItemsCommand { get; }
        public Command<Item> SelectItemCommand { get; }
        public Command<Item> DeleteItemCommand { get; }
        public Command<Item> RenameItemCommand { get; }

        private async void LoadItems()
        {
            IsBusy = true;
            Items.Clear();
            var items = await _itemService.GetSubItems(_item.Path);
            if (items != null)
            {
                foreach (var item in items)
                {
                    Items.Add(new ItemElement
                    {
                        Item = item,
                        ImageSource = ItemImageHelper.GetImageSource(item)
                    });
                }
            }

            IsBusy = false;
        }

        private async void SelectItem(Item item)
        {
            if (item != null)
            {
                if (item.IsDirectory)
                {
                    await _navigation.PushAsync(new ItemsPage(item));
                }
                else
                {
                    var content = await _itemService.DownloadFile(item.Path);
                    if (content != null)
                    {
                        byte[] bytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            await content.CopyToAsync(memoryStream);
                            bytes = memoryStream.ToArray();
                        }

                        var localPath = Path.Combine(Path.GetTempPath(), item.Name);
                        File.WriteAllBytes(localPath, bytes);
                        await Launcher.OpenAsync(new OpenFileRequest("Open with", new ReadOnlyFile(localPath)));
                    }
                }
            }
        }

        private void OpenRenameItemPopup(Item item)
        {
            PushPopupPage(new AlterItemPage(new RenameItemViewModel(PopupNavigation.Instance, item)));
        }

        private async void DeleteItem(Item item)
        {
            if (item != null)
            {
                var result = await _itemService.Delete(item.Path);

                if (result)
                {
                    OnAppearing();
                }
                else
                {
                    _message.Short("Can't process the deletion.");
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void PushPopupPage(PopupPage page)
        {
            _page = page;
            await PopupNavigation.Instance.PushAsync(_page);
        }
    }
}
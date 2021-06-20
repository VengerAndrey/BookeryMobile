using System.Collections.ObjectModel;
using System.IO;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using BookeryMobile.Models;
using BookeryMobile.Views;
using Domain.Models;
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

        public ItemsViewModel(INavigation navigation, Item item)
        {
            _navigation = navigation;
            _item = item;
            Title = item.Name;
            Items = new ObservableCollection<ItemElement>();

            LoadItemsCommand = new Command(LoadItems);
            SelectItemCommand = new Command<Item>(SelectItem);
            DeleteItemCommand = new Command<Item>(DeleteItem);
        }

        public ObservableCollection<ItemElement> Items { get; }

        public Command LoadItemsCommand { get; }
        public Command<Item> SelectItemCommand { get; }
        public Command<Item> DeleteItemCommand { get; }

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
    }
}
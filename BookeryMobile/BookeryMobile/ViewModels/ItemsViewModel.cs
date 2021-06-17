using System.Collections.ObjectModel;
using System.IO;
using BookeryApi.Services.Storage;
using BookeryMobile.Views;
using Domain.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class ItemsViewModel : BaseViewModel
    {
        private readonly Item _item;
        private readonly IItemService _itemService = DependencyService.Get<IItemService>();
        private readonly INavigation _navigation;

        public ItemsViewModel(INavigation navigation, Item item)
        {
            _navigation = navigation;
            _item = item;
            Title = item.Name;
            Items = new ObservableCollection<Item>();

            LoadItemsCommand = new Command(LoadItems);
            SelectItemCommand = new Command<Item>(OnSelectItem);
        }

        public ObservableCollection<Item> Items { get; }

        public Command LoadItemsCommand { get; }
        public Command<Item> SelectItemCommand { get; }

        private async void LoadItems()
        {
            IsBusy = true;
            Items.Clear();
            var items = await _itemService.GetSubItems(_item.Path);
            if (items != null)
            {
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }

            IsBusy = false;
        }

        private async void OnSelectItem(Item item)
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

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
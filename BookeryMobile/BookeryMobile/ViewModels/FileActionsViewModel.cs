using System;
using System.IO;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using BookeryMobile.Views;
using Domain.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class FileActionsViewModel : BaseViewModel
    {
        private readonly IItemService _itemService = DependencyService.Get<IItemService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();

        public FileActionsViewModel(Item item)
        {
            Item = item;
            OpenCommand = new Command(Open);
            DownloadCommand = new Command(Download);
        }

        public Item Item { get; set; }

        public Command OpenCommand { get; }
        public Command DownloadCommand { get; }

        private async void Open()
        {
            await PopupNavigation.Instance.PushAsync(new LoadingPage());

            var content = await _itemService.DownloadFile(Item.Path);
            if (content != null)
            {
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    await content.CopyToAsync(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                var localPath = Path.Combine(Path.GetTempPath(), Item.Name);
                File.WriteAllBytes(localPath, bytes);
                await Launcher.OpenAsync(new OpenFileRequest("Open with", new ReadOnlyFile(localPath)));
            }

            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void Download()
        {
            await PopupNavigation.Instance.PushAsync(new LoadingPage());

            var content = await _itemService.DownloadFile(Item.Path);
            if (content != null)
            {
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    await content.CopyToAsync(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                //string localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Item.Name);
                //File.WriteAllBytes(localPath, bytes);
                string localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Bookery");
                Directory.CreateDirectory(localPath);
            }

            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}

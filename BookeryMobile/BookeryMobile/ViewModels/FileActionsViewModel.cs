using System;
using System.IO;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using BookeryMobile.Services.Cache;
using BookeryMobile.Views;
using Domain.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class FileActionsViewModel : BaseViewModel
    {
        private readonly IStorageService _storageService = DependencyService.Get<IStorageService>();
        private readonly ICache _cache = DependencyService.Get<ICache>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();

        public FileActionsViewModel(Node node)
        {
            Node = node;
            Size = GetSizeString(node);
            Created = GetCreatedString(node);
            OpenCommand = new Command(Open);
            DownloadCommand = new Command(Download);
        }
        
        public Node Node { get; set; }
        
        public string Size { get; }
        
        public string Created { get; }

        public string Cached => _cache.FileExists(Node.Id.ToString()) ? "Yes" : "No";
        
        public Command OpenCommand { get; }
        public Command DownloadCommand { get; }
        
        private async void Open()
        {
            await PopupNavigation.Instance.PushAsync(new LoadingPage());

            Stream content;

            if (_cache.FileExists(Node.Id.ToString()))
            {
                content = _cache.GetFile(Node.Id.ToString());
            }
            else
            {
                content = await _storageService.Download(Node.Id);
            }
            if (content != null)
            {
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    await content.CopyToAsync(memoryStream);
                    bytes = memoryStream.ToArray();
                }
        
                var localPath = Path.Combine(Path.GetTempPath(), Node.Name);
                File.WriteAllBytes(localPath, bytes);
                await Launcher.OpenAsync(new OpenFileRequest("Open with", new ReadOnlyFile(localPath)));
            }
        
            await PopupNavigation.Instance.PopAllAsync();
        }
        
        private async void Download()
        {
            await PopupNavigation.Instance.PushAsync(new LoadingPage());
        
            var content = await _storageService.Download(Node.Id);
            if (content != null)
            {
                await _cache.SaveFile(content, Node.Id.ToString());
                _message.Short("File cached locally.");
            }
        
            await PopupNavigation.Instance.PopAllAsync();
        }
        
        private string GetSizeString(Node node)
        {
            if (node.Size < 1000)
            {
                return $"{node.Size} B";
            }
            if (node.Size < 1000 * 1000)
            {
                return $"{node.Size / 1000} KB";
            }
            if (node.Size < 1000 * 1000 * 1000)
            {
                return $"{node.Size / 1000 / 1000} MB";
            }
            return $"{node.Size / 1000 / 1000 / 1000} GB";
        }

        private string GetCreatedString(Node node)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(node.CreationTimestamp).ToLocalTime();
            return dateTime.ToString();
        }
    }
}

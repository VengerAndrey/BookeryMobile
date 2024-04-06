using System;
using System.Globalization;
using System.IO;
using BookeryMobile.Common;
using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.Services.Cache;
using BookeryMobile.Services.Storage;
using BookeryMobile.Views;
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

        public FileActionsViewModel(NodeDto node)
        {
            Node = node;
            SizeString = GetSizeString(node);
            CreatedString = GetCreatedString(node);
            OpenCommand = new Command(Open);
            DownloadCommand = new Command(Download);
            DeleteDownloadCommand = new Command(DeleteDownload, () => IsCached);
        }

        private NodeDto Node { get; set; }
        
        public string SizeString { get; }
        public string CreatedString { get; }
        public string CachedString => IsCached ? "Yes" : "No";
        private bool IsCached => _cache.FileExists(Node.Id.ToString());
        
        public Command OpenCommand { get; }
        public Command DownloadCommand { get; }
        public Command DeleteDownloadCommand { get; }
        
        private async void Open()
        {
            await PopupNavigation.Instance.PushAsync(new LoadingPage());

            Stream? content;

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

        private async void DeleteDownload()
        {
            _cache.DeleteFile(Node.Id.ToString());
            _message.Short("File removed from local cache.");
            await PopupNavigation.Instance.PopAllAsync();
        }
        
        private string GetSizeString(NodeDto node) =>
            node.Size switch
            {
                < 1000 => $"{node.Size} B",
                < 1000 * 1000 => $"{node.Size / 1000} KB",
                < 1000 * 1000 * 1000 => $"{node.Size / 1000 / 1000} MB",
                _ => $"{node.Size / 1000 / 1000 / 1000} GB"
            };

        private string GetCreatedString(NodeDto node)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(node.CreationTimestamp).ToLocalTime();
            return dateTime.ToString(CultureInfo.InvariantCulture);
        }
    }
}

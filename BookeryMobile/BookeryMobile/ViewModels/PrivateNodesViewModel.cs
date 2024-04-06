using System;
using System.Collections.ObjectModel;
using BookeryMobile.Common;
using BookeryMobile.Data.DTOs.Node.Input;
using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.Data.Models;
using BookeryMobile.Services.Node.Interfaces;
using BookeryMobile.Services.Storage;
using BookeryMobile.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class PrivateNodesViewModel : BaseViewModel
    {
        private readonly string _path;
        private readonly IPrivateNodeService _nodeService = DependencyService.Get<IPrivateNodeService>();
        private readonly IStorageService _storageService = DependencyService.Get<IStorageService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly INavigation _navigation;
        private PopupPage? _page;

        public PrivateNodesViewModel(INavigation navigation, string path)
        {
            _navigation = navigation;
            _path = path.Trim('/');
            if (_path.Length > 0)
            {
                Title = _path.Contains("/") ? _path.Substring(_path.LastIndexOf('/') + 1) : _path;
            }
            else
            {
                Title = "Private";
            }
            Nodes = new ObservableCollection<NodeElement>();

            LoadNodesCommand = new Command(LoadNodes);
            SelectNodeCommand = new Command<NodeDto>(SelectNode);
            DeleteNodeCommand = new Command<NodeDto>(DeleteNode);
            RenameNodeCommand = new Command<NodeDto>(OpenRenameNodePopup);
            CreateDirectoryCommand = new Command(OpenCreateDirectoryPopup);
            UploadFileCommand = new Command(UploadFile);
            ShareNodeCommand = new Command<NodeDto>(OpenShareNodePopup);

            PopupNavigation.Instance.Popping += (sender, args) =>
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0 && args.Page == _page)
                {
                    OnAppearing();
                }
            };
        }

        public ObservableCollection<NodeElement> Nodes { get; }

        public Command LoadNodesCommand { get; }
        public Command<NodeDto> SelectNodeCommand { get; }
        public Command<NodeDto> DeleteNodeCommand { get; }
        public Command<NodeDto> RenameNodeCommand { get; }
        public Command CreateDirectoryCommand { get; }
        public Command UploadFileCommand { get; }
        public Command<NodeDto> ShareNodeCommand { get; }

        private async void LoadNodes()
        {
            IsBusy = true;
            Nodes.Clear();
            var nodes = await _nodeService.Get(_path);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    Nodes.Add(new NodeElement
                    {
                        Node = node,
                        ImageSource = FileIconHelper.GetImageSource(node)
                    });
                }
            }

            IsBusy = false;
        }

        private async void SelectNode(NodeDto? node)
        {
            if (node != null)
            {
                if (node.IsDirectory)
                {
                    await _navigation.PushAsync(new PrivateNodesPage(_path + "/" + node.Name));
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new FileActionsPage(new FileActionsViewModel(node)));
                }
            }
        }

        private void OpenRenameNodePopup(NodeDto node)
        {
            PushPopupPage(new AlterNodePage(new RenameNodeViewModel(PopupNavigation.Instance, _nodeService, _path + '/' + node.Name, node)));
        }

        private async void DeleteNode(NodeDto? node)
        {
            if (node != null)
            {
                var result = await _nodeService.Delete((_path + "/" + node.Name).Trim('/'));

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

        private void OpenCreateDirectoryPopup()
        {
            PushPopupPage(new AlterNodePage(new CreateDirectoryViewModel(PopupNavigation.Instance, _nodeService, _path)));
        }
        
        private void OpenShareNodePopup(NodeDto node)
        {
            PushPopupPage(new ShareNodePage(node));
        }

        private async void UploadFile()
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select file"
                });
                if (file != null)
                {
                    PushPopupPage(new LoadingPage());
            
                    var fileName = file.FileName;
                    var node = await _nodeService.Create(_path, new CreateNodeDto(fileName, false));
                    if (node == null)
                    {
                        _message.Short("Problem occurred...");
                        return;
                    }
                    
                    var stream = await file.OpenReadAsync();
                    var result = await _storageService.Upload(node.Id, stream, fileName);
            
                    if (!result)
                    {
                        _message.Short("File with the same name already exists.");
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                PopPopupPage();
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

        private async void PopPopupPage()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}
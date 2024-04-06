using System;
using BookeryMobile.Common;
using BookeryMobile.Data.DTOs.Node.Input;
using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.Exceptions;
using BookeryMobile.Services.Node.Interfaces;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class RenameNodeViewModel : BaseViewModel
    {
        private readonly string _extension;
        private readonly INodeUpdateService _nodeService;
        private readonly IMessage _message = DependencyService.Get<IMessage>();
        private readonly IPopupNavigation _popupNavigation;
        private readonly string _path;

        public RenameNodeViewModel(IPopupNavigation popupNavigation, INodeUpdateService nodeService, string path,
            NodeDto node)
        {
            _popupNavigation = popupNavigation;
            _nodeService = nodeService;
            Title = "Rename";
            _path = path.Trim('/');
            _name = node.Name;
            _isDirectory = node.IsDirectory;
            _size = node.Size;
            _extension = _isDirectory || !_name.Contains(".") ? "" : _name.Substring(_name.LastIndexOf('.'));
            SubmitCommand = new Command(RenameItem, CanRenameItem);
        }

        private readonly bool _isDirectory;
        private readonly long? _size;
        private string _name;

        public string Name
        {
            get => _isDirectory || !_name.Contains(".") ? _name : _name.Substring(0, _name.LastIndexOf('.'));
            set
            {
                _name = value + _extension;
                OnPropertyChanged(nameof(Name));
                SubmitCommand.ChangeCanExecute();
            }
        }

        public Command SubmitCommand { get; }

        private async void RenameItem()
        {
            try
            {
                await _nodeService.Update(_path, new UpdateNodeDto(_name, _size, Guid.Empty));
            }
            catch (NameConflictException e)
            {
                _message.Short(e.Message);
            }
            catch (WrongAccessTypeException e)
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
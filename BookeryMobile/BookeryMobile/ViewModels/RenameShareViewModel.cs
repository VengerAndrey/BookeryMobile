using BookeryApi.Exceptions;
using BookeryApi.Services.Storage;
using BookeryMobile.Common;
using Domain.Models;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    class RenameShareViewModel : BaseViewModel
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IShareService _shareService = DependencyService.Get<IShareService>();
        private readonly IMessage _message = DependencyService.Get<IMessage>();

        public RenameShareViewModel(IPopupNavigation popupNavigation, Share share)
        {
            _popupNavigation = popupNavigation;
            Title = "Rename course";
            Share = share;
            SubmitCommand = new Command(RenameShare, CanRenameShare);
        }

        public Share Share { get; set; }

        public string Name
        {
            get => Share.Name;
            set
            {
                Share.Name = value;
                OnPropertyChanged(nameof(Name));
                SubmitCommand.ChangeCanExecute();
            }
        }

        public Command SubmitCommand { get; }

        private async void RenameShare()
        {
            try
            {
                await _shareService.Update(Share);
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

        private bool CanRenameShare()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}

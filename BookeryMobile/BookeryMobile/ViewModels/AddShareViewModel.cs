using BookeryApi.Services.Storage;
using Domain.Models;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    internal class AddShareViewModel : BaseViewModel
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IShareService _shareService = DependencyService.Get<IShareService>();

        public AddShareViewModel(IPopupNavigation popupNavigation)
        {
            _popupNavigation = popupNavigation;
            Title = "Add new course";
            Share = new Share();
            SubmitCommand = new Command(AddShare, CanAddShare);
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

        private async void AddShare()
        {
            await _shareService.Create(Share.Name);
            await _popupNavigation.PopAsync();
        }

        private bool CanAddShare()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
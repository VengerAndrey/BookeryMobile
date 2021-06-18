using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using BookeryApi.Services.Storage;
using BookeryMobile.Services.Authentication;
using BookeryMobile.Views;
using Domain.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class SharesViewModel : BaseViewModel
    {
        private readonly IAuthenticator _authenticator = DependencyService.Get<IAuthenticator>();
        private readonly INavigation _navigation;
        private readonly IShareService _shareService = DependencyService.Get<IShareService>();
        private Share _currentShare;

        private PopupPage _page;

        public SharesViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Title = "Courses";
            Shares = new ObservableCollection<Share>();
            LoadSharesCommand = new Command(async () => await LoadShares());

            SelectShareCommand = new Command<Share>(SelectShare);

            AddShareCommand = new Command(AddShare);

            PopupNavigation.Instance.Popping += (sender, args) =>
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0 && args.Page == _page)
                {
                    OnAppearing();
                }
            };

            _authenticator.StateChanged += () =>
            {
                if (!_authenticator.IsSignedIn)
                {
                }
            };
        }

        public ObservableCollection<Share> Shares { get; }
        public Command LoadSharesCommand { get; }
        public Command AddShareCommand { get; }
        public Command<Share> SelectShareCommand { get; }

        public Share CurrentShare
        {
            get => _currentShare;
            set
            {
                SetProperty(ref _currentShare, value);
                SelectShare(value);
            }
        }

        private async Task LoadShares()
        {
            IsBusy = true;

            try
            {
                Shares.Clear();
                var shares = await _shareService.GetAll();
                foreach (var share in shares)
                {
                    Shares.Add(share);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            CurrentShare = null;
        }

        private void AddShare()
        {
            PushPopupPage(new AlterSharePage());
        }

        private async void SelectShare(Share share)
        {
            if (share != null)
            {
                var item = new Item
                {
                    Name = share.Name,
                    IsDirectory = true,
                    Size = null,
                    Path = $"{share?.Id}/root"
                };

                await _navigation.PushAsync(new ItemsPage(item));
            }
        }

        private async void PushPopupPage(PopupPage page)
        {
            _page = page;
            await PopupNavigation.Instance.PushAsync(_page);
        }
    }
}
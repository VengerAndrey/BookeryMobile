using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using BookeryApi.Services.Storage;
using BookeryMobile.Views;
using Domain.Models;
using Xamarin.Forms;

namespace BookeryMobile.ViewModels
{
    public class SharesViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IShareService _shareService = DependencyService.Get<IShareService>();

        private Share _currentShare;

        public SharesViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Title = "Courses";
            Shares = new ObservableCollection<Share>();
            LoadSharesCommand = new Command(async () => await LoadShares());

            SelectShareCommand = new Command<Share>(OnShareSelected);

            AddShareCommand = new Command(OnAddShare);
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
                OnShareSelected(value);
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

        private void OnAddShare(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private async void OnShareSelected(Share share)
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


            /*IsBusy = true;

            try
            {
                Shares.Clear();
                var random = new Random();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }*/

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
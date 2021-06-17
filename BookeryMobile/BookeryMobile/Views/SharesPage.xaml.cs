using BookeryMobile.ViewModels;
using Xamarin.Forms;

namespace BookeryMobile.Views
{
    public partial class SharesPage : ContentPage
    {
        private readonly SharesViewModel _viewModel;

        public SharesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new SharesViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
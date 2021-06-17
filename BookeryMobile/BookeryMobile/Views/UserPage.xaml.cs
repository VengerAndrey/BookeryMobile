using BookeryMobile.ViewModels;
using Xamarin.Forms;

namespace BookeryMobile.Views
{
    public partial class UserPage : ContentPage
    {
        private readonly UserViewModel _viewModel;

        public UserPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new UserViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
using BookeryMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookeryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private readonly SignUpViewModel _viewModel;

        public SignUpPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SignUpViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //_viewModel.OnAppearing();
        }
    }
}
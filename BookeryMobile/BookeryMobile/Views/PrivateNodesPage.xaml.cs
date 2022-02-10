using BookeryMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookeryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivateNodesPage : ContentPage
    {
        private readonly PrivateNodesViewModel _viewModel;

        public PrivateNodesPage() : this("")
        {
            
        }
        
        public PrivateNodesPage(string path)
        {
            InitializeComponent();
            BindingContext = _viewModel = new PrivateNodesViewModel(Navigation, path);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
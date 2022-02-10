using BookeryMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookeryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SharedNodesPage : ContentPage
    {
        private readonly SharedNodesViewModel _viewModel;

        public SharedNodesPage() : this("")
        {
            
        }
        
        public SharedNodesPage(string path)
        {
            InitializeComponent();
            BindingContext = _viewModel = new SharedNodesViewModel(Navigation, path);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
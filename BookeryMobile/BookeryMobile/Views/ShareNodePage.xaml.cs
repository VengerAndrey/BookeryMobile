using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace BookeryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShareNodePage : PopupPage
    {
        private readonly ShareNodeViewModel _viewModel;
        
        public ShareNodePage(NodeDto node)
        {
            InitializeComponent();
            BindingContext = _viewModel = new ShareNodeViewModel(PopupNavigation.Instance, node);
        }

        protected override async void OnAppearing()
        {
            await _viewModel.OnAppearing();
        }
    }
}
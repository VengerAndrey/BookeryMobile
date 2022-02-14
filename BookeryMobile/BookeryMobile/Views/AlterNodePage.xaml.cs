using BookeryMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace BookeryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlterNodePage : PopupPage
    {
        public AlterNodePage(BaseViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
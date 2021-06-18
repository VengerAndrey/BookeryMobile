using BookeryMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace BookeryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlterSharePage : PopupPage
    {
        public AlterSharePage()
        {
            InitializeComponent();
            BindingContext = new AddShareViewModel(PopupNavigation.Instance);
        }
    }
}
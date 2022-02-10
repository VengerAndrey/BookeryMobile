using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookeryMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookeryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShareNodePage : PopupPage
    {
        public ShareNodePage(BaseViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
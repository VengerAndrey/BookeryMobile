using BookeryApi.Services.Storage;
using BookeryApi.Services.Token;
using BookeryApi.Services.User;
using BookeryMobile.Services.Authentication;
using Xamarin.Forms;

namespace BookeryMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<AuthenticationService>();
            DependencyService.Register<ShareService>();
            DependencyService.Register<ItemService>();
            DependencyService.Register<UserService>();
            DependencyService.Register<AccessService>();
            DependencyService.Register<PhotoService>();
            DependencyService.Register<Authenticator>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
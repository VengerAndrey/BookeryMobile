using BookeryMobile.Services.Authentication;
using BookeryMobile.Services.Authenticator;
using BookeryMobile.Services.Cache;
using BookeryMobile.Services.Node.Implementations;
using BookeryMobile.Services.Photo;
using BookeryMobile.Services.Storage;
using BookeryMobile.Services.User;
using Xamarin.Forms;

namespace BookeryMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<AuthenticationService>();
            DependencyService.Register<PrivateNodeService>();
            DependencyService.Register<SharedNodeService>();
            DependencyService.Register<StorageService>();
            DependencyService.Register<UserService>();
            DependencyService.Register<Cache>();
            DependencyService.Register<Authenticator>();
            DependencyService.Register<PhotoService>();
            DependencyService.Register<SharingService>();
            
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
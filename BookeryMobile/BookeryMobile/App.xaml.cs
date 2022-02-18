using BookeryApi.Services.Authentication;
using BookeryApi.Services.Node;
using BookeryApi.Services.Photo;
using BookeryApi.Services.Storage;
using BookeryApi.Services.User;
using BookeryMobile.Services.Authentication;
using BookeryMobile.Services.Cache;
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
            DependencyService.Register<SignUpService>();
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
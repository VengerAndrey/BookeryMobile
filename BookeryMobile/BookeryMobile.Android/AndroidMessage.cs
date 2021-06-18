using Android.App;
using Android.Widget;
using BookeryMobile.Common;
using BookeryMobile.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMessage))]
namespace BookeryMobile.Droid
{
    public class AndroidMessage : IMessage
    {
        public void Long(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long)?.Show();
        }

        public void Short(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short)?.Show();
        }
    }
}
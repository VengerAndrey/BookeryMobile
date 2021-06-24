using Android.Widget;
using BookeryMobile.Common;
using BookeryMobile.Droid;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AndroidMessage))]

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
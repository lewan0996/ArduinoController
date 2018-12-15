using Android.App;
using Android.OS;
using ArduinoController.Xamarin.Core.ViewModels;
using MvvmCross.Platforms.Android.Views;

namespace ArduinoController.Xamarin.Android.Views
{
    [Activity(Label = "Arduino controller")]
    public class DevicesView : MvxActivity<DevicesViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DevicesView);
        }
    }
}
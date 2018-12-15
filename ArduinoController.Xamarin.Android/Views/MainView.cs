using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using ArduinoController.Xamarin.Core.ViewModels;
using MvvmCross.Platforms.Android.Views;

namespace ArduinoController.Xamarin.Android.Views
{
    [Activity(Label = "Arduino controller", MainLauncher = true, LaunchMode = LaunchMode.SingleTop)]
    public class MainView : MvxActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
        }

        //public override void OnBackPressed()
        //{
        //    FinishAndRemoveTask();
        //}
    }
}
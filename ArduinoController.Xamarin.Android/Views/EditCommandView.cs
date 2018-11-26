using Android.App;
using Android.OS;
using ArduinoController.Xamarin.Core.ViewModels;
using MvvmCross.Platforms.Android.Views;

namespace ArduinoController.Xamarin.Android.Views
{
    [Activity(Label = "Arduino controller")]
    public class EditCommandView : MvxActivity<EditCommandViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditCommandView);
        }
    }
}
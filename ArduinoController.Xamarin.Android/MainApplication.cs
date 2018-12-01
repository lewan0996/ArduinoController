using System;
using Acr.UserDialogs;
using Android.App;
using Android.Runtime;
using ArduinoController.Xamarin.Core;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;

namespace ArduinoController.Xamarin.Android
{
    [Application]
    public class MainApplication : MvxAndroidApplication<MvxAndroidSetup<App>, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            UserDialogs.Init(this);
        }
    }
}
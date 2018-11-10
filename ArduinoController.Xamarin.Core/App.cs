using System.Net.Http;
using ArduinoController.Xamarin.Core.Services;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using ArduinoController.Xamarin.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<LoginViewModel>();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsSingleton();
        }
    }
}

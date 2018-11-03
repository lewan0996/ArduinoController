using ArduinoController.Xamarin.Core.ViewModels;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<LoginViewModel>();
        }
    }
}

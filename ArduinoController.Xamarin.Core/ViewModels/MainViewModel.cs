using System.Threading.Tasks;
using MvvmCross.ViewModels;
using Plugin.Settings;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private string _token;

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            Token = CrossSettings.Current.GetValueOrDefault("token", "");
        }
    }
}

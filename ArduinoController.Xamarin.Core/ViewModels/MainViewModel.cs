using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApiService _apiService;

        public MainViewModel(IMvxNavigationService navigationService, IApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        private ICommand _navigateToDevicesCommand;
        public ICommand NavigateToDevicesCommand => _navigateToDevicesCommand =
            _navigateToDevicesCommand ?? new MvxCommand(NavigateToDevices, () => true);

        private void NavigateToDevices()
        {
            _navigationService.Navigate<DevicesViewModel>();
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (!_apiService.IsLoggedIn)
            {
                await _navigationService.Navigate<LoginViewModel>();
            }
        }
    }
}

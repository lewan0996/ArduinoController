using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private ICommand _navigateToDevicesCommand;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ICommand NavigateToDevicesCommand => _navigateToDevicesCommand =
            _navigateToDevicesCommand ?? new MvxCommand(NavigateToDevices, () => true);

        private void NavigateToDevices()
        {
            _navigationService.Navigate<DevicesViewModel>();
        }
    }
}

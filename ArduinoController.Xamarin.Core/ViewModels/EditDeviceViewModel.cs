using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class EditDeviceViewModel : MvxViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMvxNavigationService _navigationService;
        public EditDeviceViewModel(IApiService apiService, IMvxNavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _macAddress;

        public string MacAddress
        {
            get => _macAddress;
            set => SetProperty(ref _macAddress, value);
        }

        private IMvxAsyncCommand _saveCommand;

        public ICommand SaveCommand => _saveCommand =
            _saveCommand ?? new MvxAsyncCommand(AddAndGoBack, () => true);

        private async Task AddAndGoBack()
        {
            await _apiService.CallAsync("devices", "POST", new { Name, MacAddress });
            await _navigationService.Navigate<DevicesViewModel>();
        }
    }
}

using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class DevicesViewModel : MvxViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMvxNavigationService _navigationService;

        public DevicesViewModel(IApiService apiService, IMvxNavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
        }

        private MvxObservableCollection<DeviceDto> _devices;
        public MvxObservableCollection<DeviceDto> Devices
        {
            get => _devices;
            set => SetProperty(ref _devices, value);
        }

        private IMvxAsyncCommand _addDeviceCommand;
        public ICommand AddDeviceCommand => _addDeviceCommand =
            _addDeviceCommand ?? new MvxAsyncCommand(NavigateToEditDevice, () => true);

        private async Task NavigateToEditDevice()
        {
            await _navigationService.Navigate<EditDeviceViewModel>();
        }

        private IMvxAsyncCommand<DeviceDto> _deleteDeviceCommand;
        public ICommand DeleteDeviceCommand => _deleteDeviceCommand =
            _deleteDeviceCommand ?? new MvxAsyncCommand<DeviceDto>(DeleteDevice, _ => true);

        private async Task DeleteDevice(DeviceDto device)
        {
            await _apiService.CallAsync($"device/{device.Id}", "DELETE");
        }

        public override void ViewAppearing()
        {
            Task.Run(async () =>
            {
                var devices = await _apiService.CallAsync<DeviceDto[]>("devices", "GET");
                Devices = new MvxObservableCollection<DeviceDto>(devices);
            });
        }
    }
}

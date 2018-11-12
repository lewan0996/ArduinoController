using System.Linq;
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

        private MvxObservableCollection<DeviceListViewItemViewModel> _devices;
        public MvxObservableCollection<DeviceListViewItemViewModel> Devices
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

        public override void ViewAppearing()
        {
            Task.Run(async () =>
            {
                var devices = await _apiService.CallAsync<DeviceDto[]>("devices", "GET");
                Devices = new MvxObservableCollection<DeviceListViewItemViewModel>(
                    devices.Select(d =>
                    {
                        var deviceListViewItemViewModel =
                            new DeviceListViewItemViewModel(_navigationService, _apiService, d);
                        deviceListViewItemViewModel.OnDeleted += (s, e) => { ViewAppearing(); };

                        return deviceListViewItemViewModel;
                    }));
            });
        }
    }
}

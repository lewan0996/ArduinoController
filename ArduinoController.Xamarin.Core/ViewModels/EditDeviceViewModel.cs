using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Exceptions;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class EditDeviceViewModel : MvxViewModel<DeviceDto>
    {
        private readonly IApiService _apiService;
        private readonly IMvxNavigationService _navigationService;
        public EditDeviceViewModel(IApiService apiService, IMvxNavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
        }

        private bool _editMode;
        private int _deviceId;

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
            _saveCommand ?? new MvxAsyncCommand(AddOrEditAndGoBack, () => true);

        private async Task AddOrEditAndGoBack()
        {
            try
            {
                if (_editMode)
                {
                    await _apiService.CallAsync($"devices/{_deviceId}", "PUT",
                        new DeviceDto {MacAddress = MacAddress, Name = Name});
                }
                else
                {
                    await _apiService.CallAsync("devices", "POST",
                        new DeviceDto {Name = Name, MacAddress = MacAddress});
                }
            }
            catch (UnsuccessfulStatusCodeException ex)
            {
                // error message
            }


            await _navigationService.Close(this);
        }

        public override void Prepare(DeviceDto device)
        {
            if (device == null)
            {
                _editMode = false;
                return;
            }
            _editMode = true;
            Name = device.Name;
            MacAddress = device.MacAddress;
            _deviceId = device.Id;
        }
    }
}

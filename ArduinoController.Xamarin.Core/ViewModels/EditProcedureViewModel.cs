using System;
using System.Threading.Tasks;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Exceptions;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class EditProcedureViewModel : MvxViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMvxNavigationService _navigationService;

        public EditProcedureViewModel(IApiService apiService, IMvxNavigationService navigationService)
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

        private DeviceDto _selectedDevice;

        public DeviceDto SelectedDevice
        {
            get => _selectedDevice;
            set => SetProperty(ref _selectedDevice, value);
        }

        private IMvxCommand<DeviceDto> _deviceSelectedCommand;

        public IMvxCommand<DeviceDto> DeviceSelectedCommand => _deviceSelectedCommand =
            _deviceSelectedCommand ?? new MvxCommand<DeviceDto>(OnDeviceSelectedCommand);

        private void OnDeviceSelectedCommand(DeviceDto device)
        {
            SelectedDevice = device;
        }

        private DeviceDto[] _devices;

        public DeviceDto[] Devices
        {
            get => _devices;
            set => SetProperty(ref _devices, value);
        }

        private async Task NavigateToEditCommand()
        {
            await _navigationService.Navigate<EditCommandViewModel>();
        }

        private IMvxAsyncCommand _addCommandCommand;

        public IMvxAsyncCommand AddCommandCommand =>
            _addCommandCommand = _addCommandCommand ?? new MvxAsyncCommand(NavigateToEditCommand);

        private IMvxAsyncCommand _addProcedureCommand;

        public IMvxAsyncCommand AddProcedureCommand =>
            _addProcedureCommand = _addProcedureCommand ?? new MvxAsyncCommand(AddProcedure);

        private async Task AddProcedure()
        {
            try
            {
                var procedure = new ProcedureDto
                {
                    DeviceId = SelectedDevice.Id,
                    Name = Name
                };

                await _apiService.CallAsync("procedures", "POST", procedure);
            }
            catch (UnsuccessfulStatusCodeException ex)
            {
                // error message
            }

            await _navigationService.Close(this);
        }

        public override void ViewAppearing()
        {
            Task.Run(() =>
            {
                try
                {
                    var devices = _apiService.CallAsync<DeviceDto[]>("devices", "GET")
                        .GetAwaiter()
                        .GetResult();
                    Devices = devices;
                }
                catch (UnsuccessfulStatusCodeException ex)
                {
                    // error message
                }
                catch (Exception ex)
                {

                }
            });
        }
    }
}

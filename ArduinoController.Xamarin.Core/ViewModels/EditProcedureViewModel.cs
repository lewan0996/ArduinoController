using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Dto.Commands;
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
        private readonly IUserDialogs _userDialogs;
        private short _order;
        private List<CommandDto> _commandsList;

        public EditProcedureViewModel(IApiService apiService, IMvxNavigationService navigationService,
            IUserDialogs userDialogs)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            _commandsList = new List<CommandDto>(); // MvxObservableCollection.ToArray throws Unimplemented Exception...
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

        private MvxObservableCollection<CommandDto> _commands;
        public MvxObservableCollection<CommandDto> Commands
        {
            get => _commands = _commands ?? new MvxObservableCollection<CommandDto>();
            set => SetProperty(ref _commands, value);
        }

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

        private async Task AddCommand()
        {
            var commandDto = await _navigationService.Navigate<EditCommandViewModel, short, CommandDto>(_order);
            Commands.Add(commandDto);
            _commandsList.Add(commandDto);
            _order++;
        }

        private IMvxAsyncCommand _addCommandCommand;

        public IMvxAsyncCommand AddCommandCommand =>
            _addCommandCommand = _addCommandCommand ?? new MvxAsyncCommand(AddCommand);

        private IMvxAsyncCommand _addProcedureCommand;

        public IMvxAsyncCommand SaveProcedureCommand =>
            _addProcedureCommand = _addProcedureCommand ?? new MvxAsyncCommand(AddProcedure);

        private async Task AddProcedure()
        {
            _userDialogs.ShowLoading();
            try
            {
                var procedure = new ProcedureDto
                {
                    DeviceId = SelectedDevice.Id,
                    Name = Name,
                    Commands = _commandsList.ToArray()
                };

                await _apiService.CallAsync("procedures", "POST", procedure);
            }
            catch (UnsuccessfulStatusCodeException ex)
            {
                await _userDialogs.AlertAsync(ex.ErrorPhrase + " " + ex.Message);
            }
            finally
            {
                _userDialogs.HideLoading();
            }

            await _navigationService.Close(this);
        }

        public override void ViewAppearing()
        {
            Task.Run(() =>
            {
                _userDialogs.ShowLoading();
                try
                {
                    var devices = _apiService.CallAsync<DeviceDto[]>("devices", "GET")
                        .GetAwaiter()
                        .GetResult();
                    Devices = devices;
                }
                catch (UnsuccessfulStatusCodeException ex)
                {
                    _userDialogs.Alert(ex.ErrorPhrase + " " + ex.Message);
                }
                catch (Exception ex)
                {
                    _userDialogs.Alert(ex.Message);
                }
                finally
                {
                    _userDialogs.HideLoading();
                }
            });
        }
    }
}

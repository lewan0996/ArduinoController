﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class DeviceListViewItemViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApiService _apiService;

        public DeviceListViewItemViewModel(IMvxNavigationService navigationService, IApiService apiService,
            DeviceDto device)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            Id = device.Id;
            MacAddress = device.MacAddress;
            Name = device.Name;
        }

        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
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

        private IMvxAsyncCommand _editDeviceCommand;

        public IMvxAsyncCommand EditDeviceCommand => _editDeviceCommand =
            _editDeviceCommand ?? new MvxAsyncCommand(NavigateToEditDevice, () => true);

        private async Task NavigateToEditDevice()
        {
            await _navigationService.Navigate<EditDeviceViewModel, DeviceDto>(new DeviceDto
            { MacAddress = MacAddress, Name = Name, Id = Id });
        }

        public event EventHandler OnDeleted;

        private IMvxAsyncCommand _deleteDeviceCommand;

        public ICommand DeleteDeviceCommand => _deleteDeviceCommand =
            _deleteDeviceCommand ?? new MvxAsyncCommand(DeleteDevice, () => true);

        private async Task DeleteDevice()
        {
            await _apiService.CallAsync($"devices/{Id}", "DELETE");
            OnDeleted?.Invoke(this, null);
        }
    }
}

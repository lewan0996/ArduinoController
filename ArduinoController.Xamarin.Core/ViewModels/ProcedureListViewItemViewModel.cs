using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class ProcedureListViewItemViewModel : MvxViewModel
    {
        private readonly IApiService _apiService;

        public ProcedureListViewItemViewModel(IApiService apiService)
        {
            _apiService = apiService;
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

        private IMvxAsyncCommand _deleteProcedureCommand;

        public ICommand DeleteProcedureCommand => _deleteProcedureCommand =
            _deleteProcedureCommand ?? new MvxAsyncCommand(DeleteDevice, () => true);

        public event EventHandler OnDeleted;

        private async Task DeleteDevice()
        {
            await _apiService.CallAsync($"procedures/{Id}", "DELETE");
            OnDeleted?.Invoke(this, null);
        }

        private IMvxAsyncCommand _runProcedureCommand;

        public ICommand RunProcedureCommand => _runProcedureCommand =
            _runProcedureCommand ?? new MvxAsyncCommand(RunProcedure, () => true);

        private async Task RunProcedure()
        {
            await _apiService.CallAsync($"procedures/{Id}/execute", "POST");
        }
    }
}

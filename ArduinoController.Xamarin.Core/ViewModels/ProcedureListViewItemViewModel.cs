using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Exceptions;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class ProcedureListViewItemViewModel : MvxViewModel
    {
        private readonly IApiService _apiService;
        private readonly IUserDialogs _userDialogs;

        public ProcedureListViewItemViewModel(IApiService apiService, ProcedureDto procedureDto,
            IUserDialogs userDialogs)
        {
            _apiService = apiService;
            _userDialogs = userDialogs;
            Id = procedureDto.Id;
            Name = procedureDto.Name;
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
            _deleteProcedureCommand ?? new MvxAsyncCommand(DeleteProcedure, () => true);

        public event EventHandler OnDeleted;

        private async Task DeleteProcedure()
        {
            _userDialogs.ShowLoading();
            try
            {
                await _apiService.CallAsync($"procedures/{Id}", "DELETE");
                OnDeleted?.Invoke(this, null);
            }
            catch (UnsuccessfulStatusCodeException ex)
            {
                _userDialogs.Alert(ex.ErrorPhrase + " " + ex.Message);
            }
            finally
            {
                _userDialogs.HideLoading();
            }
        }

        private IMvxAsyncCommand _runProcedureCommand;

        public ICommand RunProcedureCommand => _runProcedureCommand =
            _runProcedureCommand ?? new MvxAsyncCommand(RunProcedure, () => true);

        private Task RunProcedure()
        {
            return Task.Run(async () =>
            {
                _userDialogs.ShowLoading();
                try
                {
                    await _apiService.CallAsync($"procedures/{Id}/execute", "POST");
                    _userDialogs.Alert("Procedure executed successfully");
                }
                catch (UnsuccessfulStatusCodeException ex)
                {
                    _userDialogs.Alert(ex.ErrorPhrase + " " + ex.Message);
                }
                finally
                {
                    _userDialogs.HideLoading();
                }
            });

        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Exceptions;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IUserDialogs _userDialogs;

        public MainViewModel(IMvxNavigationService navigationService, IApiService apiService, IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _userDialogs = userDialogs;
        }

        private ICommand _navigateToDevicesCommand;
        public ICommand NavigateToDevicesCommand => _navigateToDevicesCommand =
            _navigateToDevicesCommand ?? new MvxAsyncCommand(NavigateToDevices, () => true);

        private async Task NavigateToEditProcedure()
        {
            await _navigationService.Navigate<EditProcedureViewModel>();
        }

        private ICommand _addProcedureCommand;
        public ICommand AddProcedureCommand => _addProcedureCommand =
            _addProcedureCommand ?? new MvxAsyncCommand(NavigateToEditProcedure, () => true);

        private async Task NavigateToDevices()
        {
            await _navigationService.Navigate<DevicesViewModel>();
        }

        private ICommand _logoutCommand;
        public ICommand LogoutCommand => _logoutCommand =
            _logoutCommand ?? new MvxAsyncCommand(Logout, () => true);

        private async Task Logout()
        {
            _apiService.Logout();
            await _navigationService.Navigate<LoginViewModel>();
        }

        private MvxObservableCollection<ProcedureListViewItemViewModel> _procedures;
        public MvxObservableCollection<ProcedureListViewItemViewModel> Procedures
        {
            get => _procedures;
            set => SetProperty(ref _procedures, value);
        }

        public override void ViewAppearing()
        {
            Task.Run(async () =>
            {
                _userDialogs.ShowLoading();
                if (!_apiService.IsLoggedIn)
                {
                    _userDialogs.HideLoading();
                    await _navigationService.Navigate<LoginViewModel>();
                    return;
                }

                try
                {
                    var procedures = await _apiService.CallAsync<ProcedureDto[]>("procedures", "GET");
                    Procedures = new MvxObservableCollection<ProcedureListViewItemViewModel>(
                        procedures.Select(p =>
                        {
                            var procedureListViewItemViewModel =
                                new ProcedureListViewItemViewModel(_apiService, p, _userDialogs);
                            procedureListViewItemViewModel.OnDeleted += (s, e) => { ViewAppearing(); };

                            return procedureListViewItemViewModel;
                        }));
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

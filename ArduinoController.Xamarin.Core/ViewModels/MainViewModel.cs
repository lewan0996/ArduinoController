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
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApiService _apiService;

        public MainViewModel(IMvxNavigationService navigationService, IApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        private ICommand _navigateToDevicesCommand;
        public ICommand NavigateToDevicesCommand => _navigateToDevicesCommand =
            _navigateToDevicesCommand ?? new MvxCommand(NavigateToDevices, () => true);

        private MvxObservableCollection<ProcedureListViewItemViewModel> _procedures;
        public MvxObservableCollection<ProcedureListViewItemViewModel> Procedures
        {
            get => _procedures;
            set => SetProperty(ref _procedures, value);
        }

        private void NavigateToDevices()
        {
            _navigationService.Navigate<DevicesViewModel>();
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (!_apiService.IsLoggedIn)
            {
                await _navigationService.Navigate<LoginViewModel>();
            }
        }

        public override void ViewAppearing()
        {
            Task.Run(async () =>
            {
                var procedures = await _apiService.CallAsync<ProcedureDto[]>("procedures", "GET");
                Procedures = new MvxObservableCollection<ProcedureListViewItemViewModel>(
                    procedures.Select(p =>
                    {
                        var procedureListViewItemViewModel =
                            new ProcedureListViewItemViewModel(_apiService, p);
                        procedureListViewItemViewModel.OnDeleted += (s, e) => { ViewAppearing(); };

                        return procedureListViewItemViewModel;
                    }));
            });
        }
    }
}

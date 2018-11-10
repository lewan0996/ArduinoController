using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Exceptions;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
// ReSharper disable UnusedMember.Global

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApiService _apiService;

        public LoginViewModel(IMvxNavigationService navigationService, IApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private IMvxAsyncCommand _loginCommand;

        public ICommand LoginCommand => _loginCommand =
            _loginCommand ?? new MvxAsyncCommand(Login, () => true);

        private async Task Login()
        {
            try
            {
                var loginDto = await _apiService.Login(Email, Password);
                await _navigationService.Navigate<MainViewModel>();
            }
            catch (UnsuccessfulStatusCodeException ex)
            {
                
            }
        }
    }
}

using System.Threading.Tasks;
using System.Windows.Input;
using ArduinoController.Xamarin.Core.Dto;
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

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private IMvxAsyncCommand _loginCommand;

        public ICommand LoginCommand => _loginCommand =
            _loginCommand ?? new MvxAsyncCommand(Login, () => !IsLoading);

        private async Task Login()
        {
            try
            {
                IsLoading = true;
                await _apiService.Login(Email, Password);
                IsLoading = false;
                await _navigationService.Navigate<MainViewModel>();
            }
            catch (UnsuccessfulStatusCodeException ex)
            {
                
            }
        }

        private IMvxAsyncCommand _registerCommand;

        public ICommand RegisterCommand => _registerCommand =
            _registerCommand ?? new MvxAsyncCommand(Register, () => !IsLoading);

        private async Task Register()
        {
            IsLoading = true;
            try
            {
                await _apiService.CallAsync("users/register", "POST", new {Email, Password});
                await _apiService.Login(Email, Password);
                IsLoading = false;
                await _navigationService.Navigate<MainViewModel>();
            }
            catch (UnsuccessfulStatusCodeException ex)
            {

            }
        }
    }
}

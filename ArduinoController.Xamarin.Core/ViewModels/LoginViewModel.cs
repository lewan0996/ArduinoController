using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
// ReSharper disable UnusedMember.Global

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        public LoginViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
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

        private IMvxAsyncCommand _navigateToRegisterCommand;

        public ICommand NavigateToRegisterCommand => _navigateToRegisterCommand =
            _navigateToRegisterCommand ?? new MvxAsyncCommand(NavigateToRegister, () => true);

        private async Task NavigateToRegister()
        {
            await _navigationService.Navigate<RegisterViewModel>();
        }
    }
}

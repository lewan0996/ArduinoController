using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
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
        private readonly IUserDialogs _userDialogs;

        public LoginViewModel(IMvxNavigationService navigationService, IApiService apiService, IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _userDialogs = userDialogs;

            _registerCommand = new MvxAsyncCommand(Register, () => !IsLoading);
            _loginCommand = new MvxAsyncCommand(Login, () => !IsLoading);
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
            set
            {
                SetProperty(ref _isLoading, value);
                _loginCommand.RaiseCanExecuteChanged();
                _registerCommand.RaiseCanExecuteChanged();
            }
        }

        private readonly IMvxAsyncCommand _loginCommand;

        public ICommand LoginCommand => _loginCommand;

        private async Task Login()
        {
            await Task.Run(async () =>
            {
                try
                {
                    IsLoading = true;
                    await _apiService.Login(Email, Password);
                    await _navigationService.Close(this);
                }
                catch (UnsuccessfulStatusCodeException ex)
                {
                    _userDialogs.Alert(ex.ErrorPhrase + " " + ex.Message);
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        private readonly IMvxAsyncCommand _registerCommand;

        public ICommand RegisterCommand => _registerCommand;

        private async Task Register()
        {
            IsLoading = true;
            try
            {
                await _apiService.CallAsync("users/register", "POST", new {Email, Password}, false);
                await _apiService.Login(Email, Password);
                await _navigationService.Navigate<MainViewModel>();
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
                IsLoading = false;
            }
        }
    }
}

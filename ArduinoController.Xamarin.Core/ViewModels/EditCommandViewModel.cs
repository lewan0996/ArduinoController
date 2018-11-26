using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class EditCommandViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private byte _pinNumber;

        public byte PinNumber
        {
            get => _pinNumber;
            set => SetProperty(ref _pinNumber, value);
        }

        private bool _value;

        public bool Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private byte _analogValue;

        public byte AnalogValue
        {
            get => _analogValue;
            set => SetProperty(ref _analogValue, value);
        }

        private int _duration;

        public int Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private IMvxAsyncCommand _saveCommandCommand;

        public IMvxAsyncCommand SaveCommandCommand =>
            _saveCommandCommand = _saveCommandCommand ?? new MvxAsyncCommand(Save);

        private async Task Save()
        {
            await _navigationService.Close(this);
        }

        public CommandTypeHolder[] CommandTypes { get; set; }

        public EditCommandViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            CommandTypes = new[]
            {
                new CommandTypeHolder
                {
                    Name = "Digital Write",
                    Type = CommandType.DigitalWrite
                },
                new CommandTypeHolder
                {
                    Name = "Analog Write",
                    Type = CommandType.AnalogWrite
                },
                new CommandTypeHolder
                {
                    Name = "Negate",
                    Type = CommandType.Negate
                },
                new CommandTypeHolder
                {
                    Name = "Wait",
                    Type = CommandType.Wait
                }
            };
        }
    }

    public class CommandTypeHolder
    {
        public string Name { get; set; }
        public CommandType Type { get; set; }
    }

    public enum CommandType
    {
        DigitalWrite = 0,
        AnalogWrite = 1,
        Negate = 2,
        Wait = 3
    }
}

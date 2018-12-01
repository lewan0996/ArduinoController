using System;
using System.Threading.Tasks;
using ArduinoController.Xamarin.Core.Dto.Commands;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ArduinoController.Xamarin.Core.ViewModels
{
    public class EditCommandViewModel : MvxViewModel<short, CommandDto>
    {
        private short _order;
        private readonly IMvxNavigationService _navigationService;

        private CommandTypeHolder _selectedType;

        public CommandTypeHolder SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }

        private IMvxCommand<CommandTypeHolder> _commandTypeSelectedCommand;

        public IMvxCommand<CommandTypeHolder> CommandTypeSelectedCommand => _commandTypeSelectedCommand =
            _commandTypeSelectedCommand ?? new MvxCommand<CommandTypeHolder>(OnCommandTypeSelected);

        private void OnCommandTypeSelected(CommandTypeHolder selectedType)
        {
            SelectedType = selectedType;
            IsDigitalWriteCommand = selectedType.Type == CommandType.DigitalWrite;
            IsAnalogWriteCommand = selectedType.Type == CommandType.AnalogWrite;
            IsWaitCommand = selectedType.Type == CommandType.Wait;
            IsNegateCommand = selectedType.Type == CommandType.Negate;
        }

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

        private bool _isWaitCommand;

        public bool IsWaitCommand
        {
            get => _isWaitCommand;
            set => SetProperty(ref _isWaitCommand, value);
        }

        private bool _isAnalogWriteCommand;

        public bool IsAnalogWriteCommand
        {
            get => _isAnalogWriteCommand;
            set => SetProperty(ref _isAnalogWriteCommand, value);
        }

        private bool _isDigitalWriteCommand;

        public bool IsDigitalWriteCommand
        {
            get => _isDigitalWriteCommand;
            set => SetProperty(ref _isDigitalWriteCommand, value);
        }

        private bool _isNegateCommand;

        public bool IsNegateCommand
        {
            get => _isNegateCommand;
            set => SetProperty(ref _isNegateCommand, value);
        }

        public override void Prepare(short order)
        {
            _order = order;
        }

        private IMvxAsyncCommand _saveCommandCommand;

        public IMvxAsyncCommand SaveCommandCommand =>
            _saveCommandCommand = _saveCommandCommand ?? new MvxAsyncCommand(Save);

        private async Task Save()
        {
            var commandDto = CreateCommandDto();
            await _navigationService.Close(this, commandDto);
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

        public CommandDto CreateCommandDto()
        {
            switch (SelectedType.Type)
            {
                case CommandType.DigitalWrite:
                    return new DigitalWriteCommandDto
                    {
                        Order = _order,
                        PinNumber = PinNumber,
                        Value = Value
                    };
                case CommandType.AnalogWrite:
                    return new AnalogWriteCommandDto
                    {
                        Order = _order,
                        PinNumber = PinNumber,
                        Value = AnalogValue
                    };
                case CommandType.Negate:
                    return new NegateCommandDto
                    {
                        Order = _order,
                        PinNumber = PinNumber
                    };
                case CommandType.Wait:
                    return new WaitCommandDto
                    {
                        Order = _order,
                        Duration = Duration
                    };
                default:
                    throw new Exception("Unknown command type");
            }
        }

        public override void ViewAppearing()
        {
            Value = false;
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

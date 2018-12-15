namespace ArduinoController.Xamarin.Core.Dto.Commands
{
    public class AnalogWriteCommandDto : CommandDto
    {
        public byte PinNumber { get; set; }
        public byte Value { get; set; }

        public override string Type => "AnalogWrite";
    }
}

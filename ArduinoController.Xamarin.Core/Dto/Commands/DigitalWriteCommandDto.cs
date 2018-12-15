namespace ArduinoController.Xamarin.Core.Dto.Commands
{
    public class DigitalWriteCommandDto : CommandDto
    {
        public byte PinNumber { get; set; }
        
        public bool Value { get; set; }

        public override string Type => "DigitalWrite";
    }
}

namespace ArduinoController.Xamarin.Core.Dto.Commands
{
    public class NegateCommandDto : CommandDto
    {
        public byte PinNumber { get; set; }

        public override string Type => "Negate";
    }
}

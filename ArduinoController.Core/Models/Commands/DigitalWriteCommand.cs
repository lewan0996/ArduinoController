namespace ArduinoController.Core.Models.Commands
{
    public class DigitalWriteCommand : Command
    {
        public byte PinNumber { get; set; }
        public bool Value { get; set; }
    }
}

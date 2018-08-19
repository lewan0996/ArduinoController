namespace ArduinoController.Core.Models.Commands
{
    public class DigitalWriteCommand : Command
    {
        public uint PinNumber { get; set; }
        public bool Value { get; set; }
    }
}

namespace ArduinoController.Core.Models.Commands
{
    public class AnalogWriteCommand : Command
    {
        public byte PinNumber { get; set; }
        public byte Value { get; set; }
    }
}

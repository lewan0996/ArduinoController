namespace ArduinoController.Core.Models.Commands
{
    public class AnalogWriteCommand : Command
    {
        public ushort PinNumber { get; set; }
        public uint Value { get; set; }
    }
}

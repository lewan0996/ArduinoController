namespace ArduinoController.Core.Models.Commands
{
    public abstract class Command
    {
        public ushort Id { get; set; }
        public ushort Order { get; set; }
    }
}

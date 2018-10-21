namespace ArduinoController.Core.Models.Commands
{
    public abstract class Command
    {
        public short Id { get; set; }
        public short Order { get; set; }
        public string Type => GetType().Name.Replace("Command", "");
    }
}

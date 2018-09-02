using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Api.Dto.Commands
{
    public class AnalogWriteCommandDto : CommandDto
    {
        public byte PinNumber { get; set; }
        public byte Value { get; set; }

        public override Command MapToCommand()
        {
            return new AnalogWriteCommand
            {
                Id = Id,
                Order = Order,
                PinNumber = PinNumber,
                Value = Value
            };
        }
    }
}

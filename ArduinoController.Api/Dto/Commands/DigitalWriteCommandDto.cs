using System.ComponentModel.DataAnnotations;
using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Api.Dto.Commands
{
    public class DigitalWriteCommandDto : CommandDto
    {
        [Required]
        public byte PinNumber { get; set; }
        [Required]
        public bool Value { get; set; }

        public override string Type => "DigitalWrite";

        public override Command MapToCommand()
        {
            return new DigitalWriteCommand
            {
                Id = Id,
                Order = Order,
                PinNumber = PinNumber,
                Value = Value
            };
        }
    }
}

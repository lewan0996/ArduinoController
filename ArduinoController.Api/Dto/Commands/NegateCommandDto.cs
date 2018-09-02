using System.ComponentModel.DataAnnotations;
using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Api.Dto.Commands
{
    public class NegateCommandDto : CommandDto
    {
        [Required]
        public byte PinNumber { get; set; }

        public override Command MapToCommand()
        {
            return new NegateCommand
            {
                Id = Id,
                Order = Order,
                PinNumber = PinNumber
            };
        }
    }
}

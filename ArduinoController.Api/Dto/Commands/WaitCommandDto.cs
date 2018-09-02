using System.ComponentModel.DataAnnotations;
using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Api.Dto.Commands
{
    public class WaitCommandDto : CommandDto
    {
        [Required]
        public int Duration { get; set; }

        public override Command MapToCommand()
        {
            return new WaitCommand
            {
                Id = Id,
                Order = Order,
                Duration = Duration
            };
        }
    }
}

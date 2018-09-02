using System.ComponentModel.DataAnnotations;
using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Api.Dto.Commands
{
    public abstract class CommandDto
    {
        public ushort Id { get; set; }
        [Required]
        public ushort Order { get; set; }

        public abstract Command MapToCommand();
    }
}

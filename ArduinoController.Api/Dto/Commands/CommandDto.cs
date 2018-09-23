using System.ComponentModel.DataAnnotations;
using ArduinoController.Core.Models.Commands;
using Newtonsoft.Json;

namespace ArduinoController.Api.Dto.Commands
{
    [JsonConverter(typeof(CommandDtoJsonConverter))]
    public abstract class CommandDto
    {
        public string Type { get; set; }
        public short Id { get; set; }
        [Required]
        public short Order { get; set; }

        public abstract Command MapToCommand();
    }
}

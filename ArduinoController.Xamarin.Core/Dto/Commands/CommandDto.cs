using ArduinoController.Xamarin.Core.Dto.Commands.Deserialization;
using Newtonsoft.Json;

namespace ArduinoController.Xamarin.Core.Dto.Commands
{
    [JsonConverter(typeof(CommandDtoJsonConverter))]
    public abstract class CommandDto
    {
        public abstract string Type { get; }
        public short Id { get; set; }
        public short Order { get; set; }
    }
}

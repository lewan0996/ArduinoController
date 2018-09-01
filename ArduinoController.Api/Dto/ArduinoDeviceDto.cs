using System.ComponentModel.DataAnnotations;

namespace ArduinoController.Api.Dto
{
    public class ArduinoDeviceDto
    {
        [Required]
        public string MacAddress { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

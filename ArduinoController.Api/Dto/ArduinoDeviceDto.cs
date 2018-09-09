using System.ComponentModel.DataAnnotations;
using ArduinoController.Core.Models;

namespace ArduinoController.Api.Dto
{
    public class ArduinoDeviceDto
    {
        [Required]
        public string MacAddress { get; set; }
        [Required]
        public string Name { get; set; }

        public ArduinoDevice MapToArduinoDevice(string userId = null) // if userId is irrelevant for the operation, it can be omitted
        {
            return new ArduinoDevice
            {
                MacAddress = MacAddress,
                Name = Name,
                UserId = userId
            };
        }
    }
}

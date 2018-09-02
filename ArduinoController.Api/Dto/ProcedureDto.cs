using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ArduinoController.Api.Dto.Commands;
using ArduinoController.Core.Models;

namespace ArduinoController.Api.Dto
{
    public class ProcedureDto
    {
        [Required]
        public string Name { get; set; }
        public ArduinoDeviceDto Device { get; set; }
        public IEnumerable<CommandDto> Commands { get; set; }

        public Procedure MapToProcedure(string userId)
        {
            return new Procedure
            {
                Name = Name,
                UserId = userId,
                Device = Device?.MapToArduinoDevice(userId),
                Commands = Commands?.Select(c => c.MapToCommand()).ToArray()
            };
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ArduinoController.Api.Dto.Commands;
using ArduinoController.Api.Dto.Commands.Validation;
using ArduinoController.Core.Models;

namespace ArduinoController.Api.Dto
{
    public class ProcedureDto
    {
        [Required]
        public string Name { get; set; }
        public int DeviceId { get; set; }
        [DistinctOrderValues]
        public IEnumerable<CommandDto> Commands { get; set; }

        public Procedure MapToProcedure(string userId = null) // if userId is irrelevant for the operation, it can be omitted
        {
            return new Procedure
            {
                Name = Name,
                UserId = userId,
                Device = new ArduinoDeviceDto { Id = DeviceId }.MapToArduinoDevice(userId),
                Commands = Commands?.Select(c => c.MapToCommand()).ToArray()
            };
        }
    }
}

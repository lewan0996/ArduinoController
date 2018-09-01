using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArduinoController.Api.Dto
{
    public class ProcedureDto
    {
        [Required]
        public string Name { get; set; }
        public string DeviceMacAddress { get; set; }
        public IEnumerable<ushort> CommandIds { get; set; }
    }
}

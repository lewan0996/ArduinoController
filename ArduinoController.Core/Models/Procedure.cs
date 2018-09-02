using System.Collections.Generic;
using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Core.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ArduinoDevice Device { get; set; }
        public virtual ICollection<Command> Commands { get; set; }
    }
}

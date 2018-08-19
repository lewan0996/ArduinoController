using System.Collections.Generic;
using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Core.Models
{
    public class Procedure
    {
        public string Name { get; set; }
        public virtual ICollection<Command> Commands { get; set; }
    }
}

using System.Collections.Generic;
using ArduinoController.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace ArduinoController.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string RefreshToken { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
        public virtual ICollection<ArduinoDevice> Devices { get; set; }
    }
}

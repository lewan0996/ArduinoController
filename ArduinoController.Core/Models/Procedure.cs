using System.Collections.Generic;
using ArduinoController.Core.Contract.Auth;
using ArduinoController.Core.Models.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArduinoController.Core.Models
{
    public class Procedure : IOwnedResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public virtual ArduinoDevice Device { get; set; }
        public virtual ICollection<Command> Commands { get; set; }

        public string GenerateDeviceMethodPayload()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}

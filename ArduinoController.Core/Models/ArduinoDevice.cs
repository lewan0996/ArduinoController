using ArduinoController.Core.Contract.Auth;

namespace ArduinoController.Core.Models
{
    public class ArduinoDevice : IOwnedResource
    {
        public int Id { get; set; }
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}

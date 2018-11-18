using ArduinoController.Xamarin.Core.Dto.Commands;

namespace ArduinoController.Xamarin.Core.Dto
{
    public class ProcedureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DeviceId { get; set; }
        public CommandDto[] Commands { get; set; }
    }
}

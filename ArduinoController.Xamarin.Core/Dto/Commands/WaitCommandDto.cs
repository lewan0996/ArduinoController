namespace ArduinoController.Xamarin.Core.Dto.Commands
{
    public class WaitCommandDto : CommandDto
    {
        public int Duration { get; set; }

        public override string Type => "Wait";
    }
}

using System.ComponentModel.DataAnnotations;

namespace ArduinoController.Api.Dto
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
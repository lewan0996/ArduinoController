using System.ComponentModel.DataAnnotations;

namespace ArduinoController.Api.Dto
{
    public class AuthDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
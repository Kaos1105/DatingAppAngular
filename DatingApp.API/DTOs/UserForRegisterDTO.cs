using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must between 6 and 20 digits")]
        public string Password { get; set; }
    }
}
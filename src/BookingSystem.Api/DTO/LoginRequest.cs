using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Api.DTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.", MinimumLength = 8)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(32, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}

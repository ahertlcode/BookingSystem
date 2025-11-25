using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace BookingSystem.Api.DTO
{
    public class SignupRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Email is required")]       
        [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.", MinimumLength = 8)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact Number is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Contact Number not valid")]
        [RegularExpression(@"^[0]\d{10}$", ErrorMessage = "Contact Number not valid")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(32, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Role { get; set; }


        private bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);

                return true;
            } catch(FormatException)
            {
                return false;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsValidEmail(Email))
            {
                yield return new ValidationResult(
                    "Email address is not valid",
                    new[] { nameof(Email) }
                );
            }

            // You can add more custom validation here if needed
        }
    }
}

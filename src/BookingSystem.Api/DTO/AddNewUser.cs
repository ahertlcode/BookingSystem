using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace BookingSystem.Api.DTO
{
    public class AddNewUser : IValidatableObject
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        private bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
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

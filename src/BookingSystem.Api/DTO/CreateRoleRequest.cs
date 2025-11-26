using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Api.DTO
{
    public class CreateRoleRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
    }
}

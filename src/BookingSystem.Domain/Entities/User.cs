namespace BookingSystem.Domain.Entities
{
   
    public class User
    {
        public string Id { get; set; }       
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public DateTime? LastLoginAt { get; set; }
    }
}
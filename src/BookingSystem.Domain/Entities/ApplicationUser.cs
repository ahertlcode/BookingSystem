

using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;


namespace BookingSystem.Domain.Entities
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string? Role { get; set; }

        public static explicit operator ApplicationUser(User v)
        {
            if (v == null) return null;

            return new ApplicationUser
            {
                Id = v.Id,
                Email = v.Email,
                UserName = v.Email,
                FullName = v.FullName,
                Role = v.Role,
                PasswordHash = v.Password
            };
        }
    }
}

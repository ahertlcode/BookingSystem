using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingSystem.Domain.Entities
{
    public class Partner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public User user { get; set; }
        public PartnerType partnerType { get; set; }
        public Sector sector { get; set; }
        public string Name { get; set; }       
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;
    }
}

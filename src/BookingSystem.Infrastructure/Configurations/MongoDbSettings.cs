namespace BookingSystem.Infrastructure.Configurations
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string Users { get; set; } = null!;
        public string Roles { get; set; } = null!;
    }

}
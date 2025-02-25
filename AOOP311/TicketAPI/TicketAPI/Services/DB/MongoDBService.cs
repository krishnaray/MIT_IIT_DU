using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using TicketAPI.Settings.DB;

namespace TicketAPI.Services.DB
{
    public class MongoDBService
    {
        readonly IMongoDatabase? _TicketDB;
        public IMongoDatabase? TicketDB => _TicketDB;
        public MongoDBService(IOptions<DBSettingsBase> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            _TicketDB = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        }

    }
}

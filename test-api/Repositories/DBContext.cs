using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using test_api.Models.Options;

namespace test_api.Repositories
{
    public class DBContext : IDisposable
    {
        private readonly IMongoDatabase _db;

        public DBContext(IOptions<MongoDBOptions> options)
        {
            var settings = options.Value;

            var client = new MongoClient(settings.ConnectionString);

            _db = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _db.GetCollection<T>(collectionName);
        }

        public void Dispose()
        {
        }
    }
}

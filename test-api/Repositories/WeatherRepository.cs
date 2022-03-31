using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using test_api.Models.Mongodb;

namespace test_api.Repositories
{
    public class WeatherRepository
    {
        protected DBContext Context { get; private set; }

        public WeatherRepository(DBContext context)
        {
            Context = context;
        }

        public async Task<IReadOnlyCollection<WeatherModel>> ListAsync()
        {
            var cursor = await Context.GetCollection<WeatherModel>("weathers").FindAsync(new BsonDocument { });

            return cursor.ToList();
        }
    }
}

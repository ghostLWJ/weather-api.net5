using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace test_api.Models.Mongodb
{
    [BsonIgnoreExtraElements]
    public class WeatherModel
    {
        public string Lat;
        public string Lon;
        public string LocationName;
        public string StationId;
        public DateTime ObsTime;
        public string Weather;
        public string Humd;
        public string Temp;
        public string City;
        public string CitySn;
        public string Town;
        public string TownSn;
    }
}

using System;
namespace test_api.Models.Responses
{
    public class WeatherResource
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string LocationName { get; set; }
        public string StationId { get; set; }
        public DateTime ObsTime { get; set; }
        public string Weather { get; set; }
        public string Humd { get; set; }
        public string Temp { get; set; }
        public string City { get; set; }
        public string CitySn { get; set; }
        public string Town { get; set; }
        public string TownSn { get; set; }
    }
}

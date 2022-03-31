using System.Collections.Generic;
using AutoMapper;
using test_api.Models.Mongodb;
using test_api.Models.Responses;

namespace test_api
{
    public class TestAPIProfile : Profile
    {
        public TestAPIProfile()
        {
            CreateMap<WeatherModel, WeatherResource>();
        }
    }
}

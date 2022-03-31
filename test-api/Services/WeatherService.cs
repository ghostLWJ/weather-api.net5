using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using test_api.Models.Responses;
using test_api.Repositories;

namespace test_api.Services
{
    public class WeatherService
    {
        private readonly WeatherRepository _weatherRepository;
        public IMapper Mapper { get; }

        public WeatherService(WeatherRepository weatherRepository, IMapper mapper)
        {

            _weatherRepository = weatherRepository;
            Mapper = mapper;
        }

        public async Task<IReadOnlyCollection<WeatherResource>> search()
        {
            var weatherModels = await _weatherRepository.ListAsync();

            return Mapper.Map<IReadOnlyCollection<WeatherResource>>(weatherModels);
        }
    }
}

using AutoMapper;
using HomeExam.AlertManagementAPI.Models;
using HomeExam.AlertManagementAPI.Models.Dto;

namespace HomeExam.AlertManagementAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UserDto, User>().ReverseMap();

                config.CreateMap<FlightDto, Flight>().ReverseMap();

                config.CreateMap<IEnumerable<UserFlight>, UserFlightsDto>()
                .ForMember(dest => dest.Flights,
                    u => u.MapFrom(src => src.Select(uf => uf.Flight).ToList()))
                .ForMember(dest => dest.User,
                    opt => opt.MapFrom(src => src.Select(uf => uf.User).FirstOrDefault()));

                config.CreateMap<UserFlightsDto, IEnumerable<UserFlight>>()
                    .ConvertUsing(dto => dto.Flights.Select(flight => new UserFlight
                    {
                        FlightId = flight.FlightId,
                        UserId = dto.User.UserId,
                        User = dto.User,
                        Flight = flight
                    }));
            });

            return mappingConfig;
        }
    }
}

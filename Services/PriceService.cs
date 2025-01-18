using AutoMapper;
using HomeExam.AlertManagementAPI.Data;
using HomeExam.AlertManagementAPI.Exceptions;
using HomeExam.AlertManagementAPI.Models;
using HomeExam.AlertManagementAPI.Models.Dto;
using HomeExam.AlertManagementAPI.RabbitMQ.RabbitMQMessageSender;
using HomeExam.AlertManagementAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace HomeExam.AlertManagementAPI.Services
{
    public class PriceService : IPriceService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPushNotificationsMessageSender _pushNotificationsMessageSender;
        private readonly IFlightService _flightService;

        public PriceService(AppDbContext db, IMapper mapper, 
            IRabbitMQPushNotificationsMessageSender pushNotificationsMessageSender
            , IFlightService flightService)
        {
            _db = db;
            _mapper = mapper;
            _pushNotificationsMessageSender = pushNotificationsMessageSender;
            _flightService = flightService;
        }

        public async Task UpdatePrice(IEnumerable<PriceAlertDto> priceAlertDtoList)
        {
            try
            {
                List<PushNotificationDto> pushNotificationDtoList = new List<PushNotificationDto>();

                foreach (var priceAlert in priceAlertDtoList)
                {
                    var response = await _flightService.GetFlight(priceAlert.FlightId);
                    
                    if (!response.IsSuccess)
                    {
                        throw new FlightNotFoundException(priceAlert.FlightId);
                    }

                    Flight flight = _mapper.Map<Flight>(response.Result);

                    flight.Price = priceAlert.Price;

                    IEnumerable<User?> userList = await _db.UsersFlights
                                                           .Where(uf => uf.FlightId == priceAlert.FlightId)
                                                           .Select(uf => uf.User)
                                                           .ToListAsync();

                    foreach (var user in userList)
                    {
                        UserDto userDto = _mapper.Map<UserDto>(user);

                        FlightDto flightDto = _mapper.Map<FlightDto>(flight);

                        PushNotificationDto pushNotificationDto = new PushNotificationDto
                        {
                            Flight = flightDto,
                            User = userDto
                        };

                        pushNotificationDtoList.Add(pushNotificationDto);
                    }
                }

                await _db.SaveChangesAsync();

                await SendPushNotifications(pushNotificationDtoList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task SendPushNotifications(IEnumerable<PushNotificationDto> pushNotificationDtoList)
        {
            if (pushNotificationDtoList.Any())
            {
                await _pushNotificationsMessageSender.SendMessage(pushNotificationDtoList);
            }
        }
    }
}

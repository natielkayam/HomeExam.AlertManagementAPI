using AutoMapper;
using HomeExam.AlertManagementAPI.Data;
using HomeExam.AlertManagementAPI.Exceptions;
using HomeExam.AlertManagementAPI.Models;
using HomeExam.AlertManagementAPI.Models.Dto;
using HomeExam.AlertManagementAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace HomeExam.AlertManagementAPI.Services
{
    public class UserFlightService : IUserFlightService
    {
        protected ResponseDto _response;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFlightService _flightService;

        public UserFlightService(AppDbContext db, IMapper mapper
            , IUserService userService, IFlightService flightService)
        {
            _response = new ResponseDto();
            _db = db;
            _mapper = mapper;
            _userService = userService;
            _flightService = flightService;
        }

        public async Task<ResponseDto> AssignFlightsToUser(UserFlightsDto userFlightDto)
        {
            try
            {
                var userCheck = await _userService.GetUser(userFlightDto.User.UserId);

                if (!userCheck.IsSuccess)
                {
                    throw new UserNotFoundException(userFlightDto.User.UserId);
                }

                var userFlightList = new List<UserFlight>();

                foreach (var f in userFlightDto.Flights)
                {
                    var flightCheck = await _flightService.GetFlight(f.FlightId);

                    if (!flightCheck.IsSuccess)
                    {
                        throw new FlightNotFoundException(f.FlightId);
                    }

                    UserFlight userFlight = new UserFlight
                    {
                        UserId = userFlightDto.User.UserId,
                        FlightId = f.FlightId,
                    };

                    userFlightList.Add(userFlight);
                }

                await _db.UsersFlights.AddRangeAsync(userFlightList);

                await _db.SaveChangesAsync();

                _response.Result = userFlightDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.Message = ex.Message.ToString();
            }
            return _response;
        }

        public async Task<ResponseDto> GetFlightsForUser(int userid)
        {
            try
            {
                var userCheck = await _userService.GetUser(userid);

                if (!userCheck.IsSuccess)
                {
                    throw new UserNotFoundException(userid);
                }

                var userFlightsList = await _db.UsersFlights
                           .Where(uf => uf.UserId == userid)
                           .Include(uf => uf.User)
                           .Include(uf => uf.Flight)
                           .ToListAsync();

                if (!userFlightsList.Any())
                {
                    throw new FlightsNotFoundForUserException(userid);
                }

                UserFlightsDto userFlightsDto = _mapper.Map<UserFlightsDto>(userFlightsList);

                _response.Result = userFlightsDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> DeleteUserFlight(int userflightid)
        {
            try
            {
                var userflight = await _db.UsersFlights.FirstOrDefaultAsync(u => u.UserFlightId == userflightid);

                if (userflight == null)
                {
                    throw new UserFlightNotFoundException(userflightid);
                }

                _db.UsersFlights.Remove(userflight);

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}

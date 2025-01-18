using AutoMapper;
using Azure;
using HomeExam.AlertManagementAPI.Data;
using HomeExam.AlertManagementAPI.Exceptions;
using HomeExam.AlertManagementAPI.Models;
using HomeExam.AlertManagementAPI.Models.Dto;
using HomeExam.AlertManagementAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace HomeExam.AlertManagementAPI.Services
{
    public class FlightService : IFlightService
    {

        protected ResponseDto _response;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public FlightService(AppDbContext db, IMapper mapper)
        {
            _response = new ResponseDto();
            _db = db;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateFlight(FlightDto flightDto)
        {
            try
            {
                Flight flight = _mapper.Map<Flight>(flightDto);

                await _db.Flights.AddAsync(flight);

                await _db.SaveChangesAsync();

                flightDto = _mapper.Map<FlightDto>(flight);

                _response.Result = flightDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.Message = ex.Message.ToString();
            }
            return _response;
        }

        public async Task<ResponseDto> GetFlight(int flightid)
        {
            try
            {
                var flight = await _db.Flights.FirstOrDefaultAsync(u => u.FlightId == flightid);

                if (flight == null)
                {
                    throw new FlightNotFoundException(flightid);
                }

                FlightDto flightDto = _mapper.Map<FlightDto>(flight);

                _response.Result = flightDto;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        
        public async Task<ResponseDto> UpdateFlight(FlightDto flightDto)
        {
            try
            {
                var existingFlight = await _db.Flights.AsNoTracking().FirstOrDefaultAsync(u => u.FlightId == flightDto.FlightId);

                if (existingFlight == null)
                {
                    throw new FlightNotFoundException(flightDto.FlightId);
                }

                Flight flight = _mapper.Map<Flight>(flightDto);

                _db.Flights.Update(flight);

                await _db.SaveChangesAsync();

                _response.Result = flightDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> DeleteFlight(int flightid)
        {
            try
            {
                var flight = await _db.Flights.FirstOrDefaultAsync(u => u.FlightId == flightid);

                if (flight == null)
                {
                    throw new FlightNotFoundException(flightid);
                }

                var userFlights = await _db.UsersFlights.Where(uf => uf.FlightId == flightid).ToListAsync();

                if (userFlights.Any())
                {
                    _db.UsersFlights.RemoveRange(userFlights);
                }

                _db.Flights.Remove(flight);

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

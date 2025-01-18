using HomeExam.AlertManagementAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HomeExam.AlertManagementAPI.Services.IServices
{
    public interface IFlightService
    {
        Task<ResponseDto> CreateFlight(FlightDto flightDto);

        Task<ResponseDto> GetFlight(int flightid);

        Task<ResponseDto> UpdateFlight(FlightDto flightDto);

        Task<ResponseDto> DeleteFlight(int flightid);
    }
}

using HomeExam.AlertManagementAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HomeExam.AlertManagementAPI.Services.IServices
{
    public interface IUserFlightService
    {
        Task<ResponseDto> AssignFlightsToUser(UserFlightsDto userFlightDto);

        Task<ResponseDto> GetFlightsForUser(int userid);

        Task<ResponseDto> DeleteUserFlight(int userflightid);
    }
}

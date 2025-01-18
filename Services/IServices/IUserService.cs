using HomeExam.AlertManagementAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HomeExam.AlertManagementAPI.Services.IServices
{
    public interface IUserService
    {
        Task<ResponseDto> CreateUser(UserDto userDto);

        Task<ResponseDto> GetUser(int userid);

        Task<ResponseDto> UpdateUser(UserDto userDto);

        Task<ResponseDto> DeleteUser(int userid);
    }
}

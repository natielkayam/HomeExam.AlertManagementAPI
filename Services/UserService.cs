using AutoMapper;
using HomeExam.AlertManagementAPI.Data;
using HomeExam.AlertManagementAPI.Exceptions;
using HomeExam.AlertManagementAPI.Models;
using HomeExam.AlertManagementAPI.Models.Dto;
using HomeExam.AlertManagementAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace HomeExam.AlertManagementAPI.Services
{
    public class UserService : IUserService
    {
        protected ResponseDto _response;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public UserService(AppDbContext db, IMapper mapper)
        {
            _response = new ResponseDto();
            _db = db;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateUser(UserDto userDto)
        {
            try
            {
                User user = _mapper.Map<User>(userDto);

                await _db.Users.AddAsync(user);

                await _db.SaveChangesAsync();

                userDto = _mapper.Map<UserDto>(user);

                _response.Result = userDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> GetUser(int userid)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userid);

                if (user == null)
                {
                    throw new UserNotFoundException(userid);
                }

                UserDto userDto = _mapper.Map<UserDto>(user);

                _response.Result = userDto;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;

                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> UpdateUser(UserDto userDto)
        {
            try
            {
                var existingUser = await _db.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userDto.UserId);

                if (existingUser == null)
                {
                    throw new UserNotFoundException(userDto.UserId);
                }

                User user = _mapper.Map<User>(userDto);

                _db.Users.Update(user);

                await _db.SaveChangesAsync();

                _response.Result = userDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.Message = ex.Message;
            }
            return _response;
        }
        
        public async Task<ResponseDto> DeleteUser(int userid)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userid);

                if (user == null)
                {
                    throw new UserNotFoundException(userid);
                }

                var userFlights = await _db.UsersFlights.Where(uf => uf.UserId == userid).ToListAsync();

                if (userFlights.Any())
                {
                    _db.UsersFlights.RemoveRange(userFlights);
                }

                _db.Users.Remove(user);

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

using AutoMapper;
using HomeExam.AlertManagementAPI.Data;
using HomeExam.AlertManagementAPI.Exceptions;
using HomeExam.AlertManagementAPI.Models;
using HomeExam.AlertManagementAPI.Models.Dto;
using HomeExam.AlertManagementAPI.RabbitMQ.RabbitMQMessageSender;
using HomeExam.AlertManagementAPI.Services;
using HomeExam.AlertManagementAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace HomeExam.AlertManagementAPI.Controllers
{
    [Route("api/alertmanagement")]
    [ApiController]
    public class AlertManagementAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IUserService _userService;
        private readonly IFlightService _flightService;
        private readonly IUserFlightService _userFlightService;

        public AlertManagementAPIController(
            IUserService userService, IFlightService flightService
            ,IUserFlightService userFlightService, IPriceService priceService)
        {
            _response = new ResponseDto();
            _userService = userService;
            _flightService = flightService;
            _userFlightService = userFlightService;
        }

        [HttpPost("CreateUser")]
        public async Task<ResponseDto> CreateUser([FromBody] UserDto userDto)
        {
            _response = await _userService.CreateUser(userDto);

            return _response;
        }

        [HttpPost("CreateFlight")]
        public async Task<ResponseDto> CreateFlight([FromBody] FlightDto flightDto)
        {
            _response = await _flightService.CreateFlight(flightDto);

            return _response;
        }

        [HttpPost("AssignFlightsToUser")]
        public async Task<ResponseDto> AssignFlightsToUser([FromBody] UserFlightsDto userFlightDto)
        {
            _response = await _userFlightService.AssignFlightsToUser(userFlightDto);

            return _response;
        }

        [HttpGet("GetUser/{userid:int}")]
        public async Task<ResponseDto> GetUser(int userid)
        {
            _response = await _userService.GetUser(userid);

            return _response;
        }

        [HttpGet("GetFlight/{flightid:int}")]
        public async Task<ResponseDto> GetFlight(int flightid)
        {
            _response = await _flightService.GetFlight(flightid);

            return _response;
        }

        [HttpGet("GetFlightsForUser/{userid:int}")]
        public async Task<ResponseDto> GetFlightsForUser(int userid)
        {
            _response = await _userFlightService.GetFlightsForUser(userid);

            return _response;
        }

        [HttpPut("UpdateUser")]
        public async Task<ResponseDto> UpdateUser([FromBody] UserDto userDto)
        {
            _response = await _userService.UpdateUser(userDto);
            
            return _response;
        }

        [HttpPut("UpdateFlight")]
        public async Task<ResponseDto> UpdateFlight([FromBody] FlightDto flightDto)
        {
            _response = await _flightService.UpdateFlight(flightDto);

            return _response;
        }

        [HttpDelete("DeleteUser/{userid:int}")]
        public async Task<ResponseDto> DeleteUser(int userid)
        {
            _response = await _userService.DeleteUser(userid);

            return _response;
        }

        [HttpDelete("DeleteFlight/{flightid:int}")]
        public async Task<ResponseDto> DeleteFlight(int flightid)
        {
            _response = await _flightService.DeleteFlight(flightid);

            return _response;
        }

        [HttpDelete("DeleteUserFlight/{userflightid:int}")]
        public async Task<ResponseDto> DeleteUserFlight(int userflightid)
        {
            _response = await _userFlightService.DeleteUserFlight(userflightid);

            return _response;
        }

    }
}

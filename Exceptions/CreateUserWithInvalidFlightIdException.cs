using HomeExam.AlertManagementAPI.Models.Dto;
using HomeExam.AlertManagementAPI.Models;

namespace HomeExam.AlertManagementAPI.Exceptions
{
    public class CreateUserWithInvalidFlightIdException : Exception
    {
        public CreateUserWithInvalidFlightIdException(int userId, int flightId)
            : base($"Cannot add user with UserId {userId} to a non-existent FlightId {flightId}.")
        {}
    }
}

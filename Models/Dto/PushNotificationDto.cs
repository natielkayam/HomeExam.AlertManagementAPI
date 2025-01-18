namespace HomeExam.AlertManagementAPI.Models.Dto
{
    public class PushNotificationDto
    {
        public UserDto? User { get; set; }

        public FlightDto? Flight { get; set; }
    }
}

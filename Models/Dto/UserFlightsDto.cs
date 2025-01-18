namespace HomeExam.AlertManagementAPI.Models.Dto
{
    public class UserFlightsDto
    {
        public IEnumerable<Flight>? Flights { get; set; }

        public User? User { get; set; }
    }
}

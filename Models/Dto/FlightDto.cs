
namespace HomeExam.AlertManagementAPI.Models.Dto
{
    public class FlightDto
    {
        public int FlightId { get; set; }

        public double Price { get; set; }

        public string? FlightName { get; set; }

        public string? Destination { get; set; }

        public string? Departure { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HomeExam.AlertManagementAPI.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        public double Price { get; set; }

        [Required] 
        public string? FlightName { get; set; }

        [Required]
        public string? Destination { get; set; }

        [Required]
        public string? Departure { get; set; }
    }
}

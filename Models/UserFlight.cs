using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeExam.AlertManagementAPI.Models
{
    public class UserFlight
    {
        [Key]
        public int UserFlightId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}

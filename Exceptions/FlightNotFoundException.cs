namespace HomeExam.AlertManagementAPI.Exceptions
{
    public class FlightNotFoundException : Exception
    {
        public FlightNotFoundException(int flightid)
            : base($"Flight {flightid} not found")
        { }
    }
}

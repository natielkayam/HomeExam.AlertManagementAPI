namespace HomeExam.AlertManagementAPI.Exceptions
{
    public class FlightsNotFoundForUserException : Exception
    {
        public FlightsNotFoundForUserException(int userid)
            : base($"Flights not found for user {userid}")
        { }
    }
}

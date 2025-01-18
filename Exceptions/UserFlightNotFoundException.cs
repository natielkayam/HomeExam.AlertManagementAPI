namespace HomeExam.AlertManagementAPI.Exceptions
{
    public class UserFlightNotFoundException : Exception
    {
        public UserFlightNotFoundException(int userflightid)
            : base($"User flight id {userflightid} not found")
        { }
    }
}

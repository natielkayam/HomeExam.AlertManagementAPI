namespace HomeExam.AlertManagementAPI.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId) 
            :base($"User {userId} not found")
        { }
    }
}

namespace webNET_Hits_backend_aspnet_project_1.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message)
    { }
}
namespace webNET_Hits_backend_aspnet_project_1.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message)
    { }
}
namespace webNET_Hits_backend_aspnet_project_1.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException(string message) : base(message)
    { }
}
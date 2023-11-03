namespace webNET_Hits_backend_aspnet_project_1.Exceptions;

public class WrongPageNumException : Exception
{
    public WrongPageNumException(string message) : base(message) { }
}
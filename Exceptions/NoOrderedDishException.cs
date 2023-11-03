namespace webNET_Hits_backend_aspnet_project_1.Exceptions;

public class NoOrderedDishException : Exception
{
    public NoOrderedDishException(string message) : base(message) { }
}
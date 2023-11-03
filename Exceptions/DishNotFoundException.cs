namespace webNET_Hits_backend_aspnet_project_1.Exceptions;

public class DishNotFoundException : Exception
{
    public DishNotFoundException(string message) : base(message) { }
}
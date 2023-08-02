namespace Service_Bus_TMS.API.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}
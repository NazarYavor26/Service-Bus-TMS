using System;

namespace Service_Bus_TMS.BLL.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}
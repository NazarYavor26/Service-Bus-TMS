using System;

namespace Service_Bus_TMS.BLL.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
using System;
namespace Machine.Application.Common.Exceptions;

public class InvalidStateException : Exception
{
    public InvalidStateException() : base("Action is not valid in this state")
    { }

    public InvalidStateException(string message) : base(message)
    { }

    public InvalidStateException(string message, Exception innerException) : base(message, innerException)
    { }
}
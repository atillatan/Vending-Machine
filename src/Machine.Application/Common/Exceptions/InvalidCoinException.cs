using System;
namespace Machine.Application.Common.Exceptions;

public class InvalidCoinException : Exception
{
    public InvalidCoinException() : base("Coin is not valid!")
    { }

    public InvalidCoinException(string message) : base(message)
    { }

    public InvalidCoinException(string message, Exception innerException) : base(message, innerException)
    { }
}
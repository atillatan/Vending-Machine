using System;
namespace Machine.Application.Common.Exceptions;

public class InvalidProductException : Exception
{
    public InvalidProductException() : base("Product is not valid!")
    { }

    public InvalidProductException(string message) : base(message)
    { }

    public InvalidProductException(string message, Exception innerException) : base(message, innerException)
    { }
}
using System;
namespace Machine.Application.Common.Exceptions;

public class ExactChangeOnlyException : Exception
{
    public ExactChangeOnlyException() : base("EXACT CHANGE ONLY")
    { }

    public ExactChangeOnlyException(string message) : base(message)
    { }

    public ExactChangeOnlyException(string message, Exception innerException) : base(message, innerException)
    { }
}
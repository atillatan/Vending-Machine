using FluentValidation.Results;
namespace Machine.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string defaultMessage) : base(defaultMessage)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures, string defaultMessage)
        : this(defaultMessage)
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}

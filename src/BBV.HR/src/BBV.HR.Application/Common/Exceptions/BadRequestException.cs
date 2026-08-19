namespace BBV.HR.Application.Common.Exceptions;

public class BadRequestException : Exception
{
    public IEnumerable<string>? Errors { get; }

    public BadRequestException(string message, IEnumerable<string>? errors = null) : base(message)
    {
        Errors = errors;
    }
}

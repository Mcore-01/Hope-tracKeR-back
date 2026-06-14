using FluentResults;

namespace Hope_tracKeR_back.Errors;

public class InvalidOperationError : Error
{
    public InvalidOperationError(string message) : base(message) { }
}
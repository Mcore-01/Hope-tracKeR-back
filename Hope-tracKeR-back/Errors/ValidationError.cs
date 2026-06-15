using FluentResults;

namespace Hope_tracKeR_back.Errors;

public class ValidationError : Error
{
	public ValidationError(string message) : base(message) { }
}
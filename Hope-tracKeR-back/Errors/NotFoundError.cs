using FluentResults;

namespace Hope_tracKeR_back.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string entityName, int id) : base($"Объект {entityName} с ID {id} не найден!")
    {
        Metadata.Add("Class", entityName);
        Metadata.Add("Id", id);
    }

    public NotFoundError(string message) : base(message) { }
}
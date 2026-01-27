namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

public class EntityNotFoundException : KeyNotFoundException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.")
    {
    }
}

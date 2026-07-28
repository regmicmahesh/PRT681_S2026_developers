namespace Domain.Exceptions;

public class EntityNotFoundException(string entityName, Guid id)
    : Exception($"Entity \"{entityName}\" ({id}) was not found.")
{
}

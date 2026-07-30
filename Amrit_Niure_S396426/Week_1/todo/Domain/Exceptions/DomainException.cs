namespace Domain.Exceptions;

/// <summary>
/// Thrown when an operation would violate a business rule (an "invariant") owned by the domain model.
/// </summary>
public class DomainException(string message) : Exception(message)
{
}

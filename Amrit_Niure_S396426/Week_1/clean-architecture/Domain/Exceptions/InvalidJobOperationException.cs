namespace Domain.Exceptions;

public class InvalidJobOperationException : DomainException
{
    public InvalidJobOperationException(string message) : base(message)
    {
    }
}

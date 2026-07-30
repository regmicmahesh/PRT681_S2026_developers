namespace Domain.Exceptions;

public class ApplicationAlreadyExistsException : DomainException
{
    public ApplicationAlreadyExistsException(Guid jobSeekerId, Guid jobPostId)
        : base($"Job seeker '{jobSeekerId}' has already submitted an application for job post '{jobPostId}'.")
    {
    }
}

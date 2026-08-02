using Domain.Common;

namespace Domain
{
    public class Company : AggregateRoot
    {
        public string Name { get; private set; }

        public Company(Guid id, string name) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Name = name;
        }
    }
}

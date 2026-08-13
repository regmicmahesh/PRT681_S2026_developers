using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JobBoard.Persistence;

// Used only by `dotnet ef` at design time to generate migrations without a running host.
public sealed class JobBoardDbContextFactory : IDesignTimeDbContextFactory<JobBoardDbContext>
{
    public JobBoardDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<JobBoardDbContext>();
        optionsBuilder.UseSqlite("Data Source=jobboard.db");

        return new JobBoardDbContext(optionsBuilder.Options, new NullPublisher());
    }

    private sealed class NullPublisher : IPublisher
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification => Task.CompletedTask;
    }
}

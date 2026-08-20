var builder = DistributedApplication.CreateBuilder(args);

var pgPassword = builder.AddParameter("postgres-password", secret: true);

var authDb = builder.AddPostgres("database", password: pgPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHostPort(5435)
    .AddDatabase("auth-db");

builder.AddProject<Projects.Auth>("auth")
    .WithReference(authDb)
    .WaitFor(authDb);

builder.Build().Run();

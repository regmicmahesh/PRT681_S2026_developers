var builder = DistributedApplication.CreateBuilder(args);

var pgPassword = builder.AddParameter("postgres-password", secret: true);

var usersDb = builder.AddPostgres("database", password: pgPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHostPort(5432)
    .AddDatabase("users-db");

builder.AddProject<Projects.authentication>("authentication-api")
    .WithReference(usersDb)
    .WaitFor(usersDb);

builder.Build().Run();

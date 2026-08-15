var builder = DistributedApplication.CreateBuilder(args);

var usersDb = builder.AddPostgres("database")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHostPort(5432)
    .AddDatabase("users-db");

builder.AddProject<Projects.authentication>("authentication-api")
    .WithReference(usersDb)
    .WaitFor(usersDb);

builder.Build().Run();

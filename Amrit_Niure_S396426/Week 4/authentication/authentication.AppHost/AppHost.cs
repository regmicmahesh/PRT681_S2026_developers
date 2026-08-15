var builder = DistributedApplication.CreateBuilder(args);



builder.AddProject<Projects.authentication>("authentication");

builder.Build().Run();

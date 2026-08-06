namespace CleanApp.WebApi.IntegrationTests;

/// <summary>
/// Program.cs assigns Serilog's static Log.Logger and later calls Log.CloseAndFlush(),
/// and WebApplicationFactory boots Program's entry point in-process. Two factory
/// instances starting concurrently (xUnit runs different test classes in parallel by
/// default) race on that shared static state. Grouping every test class into one
/// collection makes xUnit share a single factory and run them sequentially instead.
/// </summary>
[CollectionDefinition(nameof(WebApiTestCollection))]
public sealed class WebApiTestCollection : ICollectionFixture<CleanAppWebApplicationFactory>;

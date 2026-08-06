using System.Runtime.CompilerServices;

// Command/query handlers are `internal` (they're only ever invoked through MediatR),
// but tests need to construct them directly to unit-test handler logic in isolation.
[assembly: InternalsVisibleTo("CleanApp.Application.Tests")]

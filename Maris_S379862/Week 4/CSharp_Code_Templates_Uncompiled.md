# Week 4 — C# Code Templates (Uncompiled Research)

These concise templates map the lecturer's requested patterns. They are intentionally labelled **uncompiled** because the verified environment has no .NET SDK. Before using them, create a real project, add tests and replace placeholders through secure configuration.

## Read a text file asynchronously

```csharp
string content = await File.ReadAllTextAsync(path, cancellationToken);
```

Validate/resolve paths against an approved directory when `path` is influenced by a user.

## Stream a file from an ASP.NET endpoint

```csharp
return Results.File(
    fileStream,
    contentType: "application/pdf",
    fileDownloadName: "report.pdf");
```

Use an allowlist/source record; never expose arbitrary server paths.

## Read CSV without a third-party parser

```csharp
foreach (string line in await File.ReadAllLinesAsync(path, cancellationToken))
{
    string[] columns = line.Split(','); // Only safe for deliberately simple CSV.
}
```

Real CSV may contain quoted commas/newlines; use a reviewed library for production data.

## Return a generated CSV safely

```csharp
static string CsvCell(string value)
{
    string safe = value.Length > 0 && "=+-@".Contains(value[0]) ? $"'{value}" : value;
    return $"\"{safe.Replace("\"", "\"\"")}\"";
}

string csv = string.Join(Environment.NewLine, rows.Select(row =>
    string.Join(",", CsvCell(row.Company), CsvCell(row.Role))));

return Results.Text(csv, "text/csv; charset=utf-8");
```

The leading-character guard reduces spreadsheet-formula injection risk.

## Send an email

```csharp
await emailSender.SendAsync(
    recipient,
    subject,
    plainTextBody,
    cancellationToken);
```

Use a provider abstraction, validate recipients, encode content, rate-limit and keep credentials in managed secrets—not source code.

## Call a web API

```csharp
using HttpResponseMessage response = await httpClient.GetAsync(
    "api/applications",
    cancellationToken);
response.EnsureSuccessStatusCode();
ApplicationDto[] items =
    await response.Content.ReadFromJsonAsync<ApplicationDto[]>(cancellationToken)
    ?? [];
```

Use `IHttpClientFactory`, timeouts/retry policy appropriate to idempotency and validation of external responses.

## Publish a message

```csharp
await messagePublisher.PublishAsync(
    new ApplicationCreated(application.Id),
    cancellationToken);
```

Define delivery/idempotency and consider an outbox when database state and messages must remain consistent.

## Consume a message

```csharp
public async Task HandleAsync(ApplicationCreated message, CancellationToken token)
{
    if (await processedMessages.ExistsAsync(message.MessageId, token)) return;
    await followUpService.ScheduleAsync(message.ApplicationId, token);
    await processedMessages.RecordAsync(message.MessageId, token);
}
```

Consumers should tolerate redelivery and send repeatedly failing messages to a dead-letter/poison-message path.

## Async method pattern

```csharp
public async Task<ApplicationDto?> GetAsync(int id, CancellationToken token)
{
    return await db.Applications
        .AsNoTracking()
        .Where(item => item.Id == id)
        .Select(item => new ApplicationDto(item.Id, item.Company, item.Role))
        .SingleOrDefaultAsync(token);
}
```

Pass cancellation, avoid blocking `.Result`/`.Wait()`, project only required fields and use async APIs for I/O.

## Verification backlog

- [ ] Install supported .NET SDK and record `dotnet --info`.
- [ ] Place each pattern in a real project.
- [ ] Add success, invalid-input, cancellation and failure-path tests.
- [ ] Run formatter/build/tests.
- [ ] Replace conceptual abstractions with chosen libraries and document versions/licences.

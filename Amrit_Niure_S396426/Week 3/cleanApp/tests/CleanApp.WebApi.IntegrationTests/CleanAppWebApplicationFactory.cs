using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CleanApp.WebApi.IntegrationTests;

/// <summary>
/// Boots the real WebApi host (full DI graph, real SQLite + migrations, real Hangfire)
/// against a unique temp SQLite file per test class instance, so tests never touch the
/// dev database and don't interfere with each other.
/// </summary>
public sealed class CleanAppWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string SeededAdminEmail = "admin@test.local";
    public const string SeededAdminPassword = "AdminTest#123";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"cleanapp-tests-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = $"Data Source={_dbPath}",
                ["Email:Enabled"] = "false",
                ["AdminUser:Email"] = SeededAdminEmail,
                ["AdminUser:Password"] = SeededAdminPassword
            });
        });
    }

    /// <summary>Registers a brand-new user and returns a client with its Bearer token attached.</summary>
    public async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        var client = CreateClient();

        var email = $"{Guid.NewGuid():N}@example.com";
        var response = await client.PostAsJsonAsync("/api/auth/register", new { email, password = "Passw0rd!" });
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        return client;
    }

    /// <summary>Logs in as the admin account seeded on startup and returns a client with its Bearer token attached.</summary>
    public async Task<HttpClient> CreateAdminClientAsync()
    {
        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new { email = SeededAdminEmail, password = SeededAdminPassword });
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        return client;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (File.Exists(_dbPath))
            File.Delete(_dbPath);
    }

    private sealed record AuthResponseDto(Guid UserId, string Email, string Token, DateTime ExpiresAtUtc);
}

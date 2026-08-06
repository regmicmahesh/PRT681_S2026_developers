using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CleanApp.Application.Admin;

namespace CleanApp.WebApi.IntegrationTests;

[Collection(nameof(WebApiTestCollection))]
public class AdminEndpointsTests(CleanAppWebApplicationFactory factory) : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private HttpClient _regularUserClient = null!;

    public async Task InitializeAsync() => _regularUserClient = await factory.CreateAuthenticatedClientAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetAllTodoLists_AsRegularUser_ReturnsForbidden()
    {
        var response = await _regularUserClient.GetAsync("/api/admin/todo-lists");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAllTodoLists_Unauthenticated_ReturnsUnauthorized()
    {
        using var anonymousClient = factory.CreateClient();

        var response = await anonymousClient.GetAsync("/api/admin/todo-lists");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAllTodoLists_AsAdmin_SeesOtherUsersLists()
    {
        var createResponse = await _regularUserClient.PostAsJsonAsync(
            "/api/todo-lists", new { title = "A Regular User's List", colour = (string?)null });
        createResponse.EnsureSuccessStatusCode();

        using var adminClient = await factory.CreateAdminClientAsync();
        var response = await adminClient.GetAsync("/api/admin/todo-lists");
        response.EnsureSuccessStatusCode();

        var lists = await response.Content.ReadFromJsonAsync<List<AdminTodoListResponse>>(JsonOptions);
        Assert.Contains(lists!, l => l.Title == "A Regular User's List");
    }

    [Fact]
    public async Task GetTodoListById_AsAdmin_ReturnsAnyUsersListWithOwnerEmail()
    {
        var createResponse = await _regularUserClient.PostAsJsonAsync(
            "/api/todo-lists", new { title = "Support Escalation", colour = (string?)null });
        var created = await createResponse.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);

        using var adminClient = await factory.CreateAdminClientAsync();
        var response = await adminClient.GetAsync($"/api/admin/todo-lists/{created!.Id}");
        response.EnsureSuccessStatusCode();

        var detail = await response.Content.ReadFromJsonAsync<AdminTodoListDetailResponse>(JsonOptions);
        Assert.Equal("Support Escalation", detail!.Title);
        Assert.NotEmpty(detail.OwnerEmail);
    }

    private sealed record IdResponse(Guid Id);
}

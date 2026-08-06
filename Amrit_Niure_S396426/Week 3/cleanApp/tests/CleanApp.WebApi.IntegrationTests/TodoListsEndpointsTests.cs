using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CleanApp.Application.TodoLists;

namespace CleanApp.WebApi.IntegrationTests;

[Collection(nameof(WebApiTestCollection))]
public class TodoListsEndpointsTests(CleanAppWebApplicationFactory factory) : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private HttpClient _client = null!;

    public async Task InitializeAsync() => _client = await factory.CreateAuthenticatedClientAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Get_WithoutAuthentication_ReturnsUnauthorized()
    {
        using var anonymousClient = factory.CreateClient();

        var response = await anonymousClient.GetAsync("/api/todo-lists");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidTitle_ReturnsCreated()
    {
        var response = await _client.PostAsJsonAsync("/api/todo-lists", new { title = "Groceries", colour = (string?)null });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_WithEmptyTitle_ReturnsBadRequestWithValidationErrors()
    {
        var response = await _client.PostAsJsonAsync("/api/todo-lists", new { title = "", colour = (string?)null });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Title", body);
    }

    [Fact]
    public async Task GetById_WhenListDoesNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/todo-lists/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenListBelongsToAnotherUser_ReturnsNotFound()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/todo-lists", new { title = "My Private List", colour = (string?)null });
        var created = await createResponse.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);

        using var otherUsersClient = await factory.CreateAuthenticatedClientAsync();
        var response = await otherUsersClient.GetAsync($"/api/todo-lists/{created!.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_OnlyReturnsListsOwnedByCurrentUser()
    {
        await _client.PostAsJsonAsync("/api/todo-lists", new { title = "Mine", colour = (string?)null });

        using var otherUsersClient = await factory.CreateAuthenticatedClientAsync();
        var response = await otherUsersClient.GetAsync("/api/todo-lists");
        response.EnsureSuccessStatusCode();

        var lists = await response.Content.ReadFromJsonAsync<List<TodoListResponse>>(JsonOptions);
        Assert.DoesNotContain(lists!, l => l.Title == "Mine");
    }

    [Fact]
    public async Task FullLifecycle_CreateRenameGetDelete_Succeeds()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/todo-lists", new { title = "Chores", colour = (string?)null });
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);

        var renameResponse = await _client.PutAsJsonAsync($"/api/todo-lists/{created!.Id}", new { title = "Household Chores" });
        Assert.Equal(HttpStatusCode.NoContent, renameResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/todo-lists/{created.Id}");
        getResponse.EnsureSuccessStatusCode();
        var detail = await getResponse.Content.ReadFromJsonAsync<TodoListDetailResponse>(JsonOptions);
        Assert.Equal("Household Chores", detail!.Title);
        Assert.Empty(detail.Items);

        var deleteResponse = await _client.DeleteAsync($"/api/todo-lists/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getAfterDelete = await _client.GetAsync($"/api/todo-lists/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
    }

    private sealed record IdResponse(Guid Id);
}

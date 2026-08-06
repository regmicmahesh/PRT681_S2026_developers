using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CleanApp.Application.TodoItems;

namespace CleanApp.WebApi.IntegrationTests;

[Collection(nameof(WebApiTestCollection))]
public class TodoItemsEndpointsTests(CleanAppWebApplicationFactory factory) : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private HttpClient _client = null!;

    public async Task InitializeAsync() => _client = await factory.CreateAuthenticatedClientAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<Guid> CreateListAsync(HttpClient client, string title = "Groceries")
    {
        var response = await client.PostAsJsonAsync("/api/todo-lists", new { title, colour = (string?)null });
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);
        return created!.Id;
    }

    private async Task<Guid> CreateItemAsync(HttpClient client, Guid listId, string title = "Buy milk")
    {
        var response = await client.PostAsJsonAsync("/api/todo-items", new
        {
            todoListId = listId,
            title,
            priority = 1,
            note = (string?)null,
            reminderUtc = (DateTime?)null
        });
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);
        return created!.Id;
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var listId = await CreateListAsync(_client);

        var response = await _client.PostAsJsonAsync("/api/todo-items", new
        {
            todoListId = listId,
            title = "Buy milk",
            priority = 1,
            note = (string?)null,
            reminderUtc = (DateTime?)null
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_ForNonExistentList_ReturnsNotFound()
    {
        var response = await _client.PostAsJsonAsync("/api/todo-items", new
        {
            todoListId = Guid.NewGuid(),
            title = "Buy milk",
            priority = 1,
            note = (string?)null,
            reminderUtc = (DateTime?)null
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ForAnotherUsersList_ReturnsNotFound()
    {
        using var otherUsersClient = await factory.CreateAuthenticatedClientAsync();
        var otherUsersListId = await CreateListAsync(otherUsersClient);

        var response = await _client.PostAsJsonAsync("/api/todo-items", new
        {
            todoListId = otherUsersListId,
            title = "Buy milk",
            priority = 1,
            note = (string?)null,
            reminderUtc = (DateTime?)null
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CompleteThenCompleteAgain_ReturnsConflictSecondTime()
    {
        var listId = await CreateListAsync(_client);
        var itemId = await CreateItemAsync(_client, listId);

        var firstComplete = await _client.PostAsync($"/api/todo-items/{itemId}/complete", null);
        Assert.Equal(HttpStatusCode.NoContent, firstComplete.StatusCode);

        var secondComplete = await _client.PostAsync($"/api/todo-items/{itemId}/complete", null);
        Assert.Equal(HttpStatusCode.Conflict, secondComplete.StatusCode);
    }

    [Fact]
    public async Task Complete_ForAnotherUsersItem_ReturnsNotFound()
    {
        using var otherUsersClient = await factory.CreateAuthenticatedClientAsync();
        var otherUsersListId = await CreateListAsync(otherUsersClient);
        var otherUsersItemId = await CreateItemAsync(otherUsersClient, otherUsersListId);

        var response = await _client.PostAsync($"/api/todo-items/{otherUsersItemId}/complete", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByList_ReturnsCreatedItem()
    {
        var listId = await CreateListAsync(_client);
        await CreateItemAsync(_client, listId);

        var response = await _client.GetAsync($"/api/todo-items?todoListId={listId}");
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<List<TodoItemResponse>>(JsonOptions);
        var item = Assert.Single(items!);
        Assert.Equal("Buy milk", item.Title);
    }

    private sealed record IdResponse(Guid Id);
}

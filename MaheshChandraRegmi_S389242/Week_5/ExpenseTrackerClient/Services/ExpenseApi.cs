using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ExpenseTrackerClient.Models;

namespace ExpenseTrackerClient.Services
{
    public class ExpenseApi
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _http;
        private readonly AuthState _auth;

        public ExpenseApi(HttpClient http, AuthState auth)
        {
            _http = http;
            _auth = auth;
        }

        public async Task<AuthResponse?> RegisterAsync(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", new RegisterRequest(email, password));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        }

        public async Task<AuthResponse?> LoginAsync(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new LoginRequest(email, password));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        }

        public async Task<List<ExpenseItem>> GetExpensesAsync(decimal? minAmount = null)
        {
            var url = minAmount is null ? "api/expenses" : $"api/expenses?minAmount={minAmount}";
            return await GetAsync<List<ExpenseItem>>(url) ?? [];
        }

        public async Task<ExpenseSummary?> GetSummaryAsync() => await GetAsync<ExpenseSummary>("api/expenses/summary");

        public async Task<List<CategoryItem>> GetCategoriesAsync() =>
            await GetAsync<List<CategoryItem>>("api/categories") ?? [];

        public async Task<ExpenseItem?> AddExpenseAsync(string description, decimal amount, int categoryId)
        {
            ApplyAuth();
            var response = await _http.PostAsJsonAsync(
                "api/expenses",
                new CreateExpenseRequest(description, amount, categoryId)
            );

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ExpenseItem>(JsonOptions);
        }

        public async Task<bool> DeleteExpenseAsync(Guid id)
        {
            ApplyAuth();
            var response = await _http.DeleteAsync($"api/expenses/{id}");
            return response.IsSuccessStatusCode;
        }

        private async Task<T?> GetAsync<T>(string url)
        {
            ApplyAuth();
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        }

        private void ApplyAuth()
        {
            _http.DefaultRequestHeaders.Authorization = _auth.Token is null
                ? null
                : new AuthenticationHeaderValue("Bearer", _auth.Token);
        }
    }
}

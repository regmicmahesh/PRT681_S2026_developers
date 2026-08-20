using Microsoft.JSInterop;

namespace ExpenseTrackerClient.Services
{
    public class AuthState
    {
        public string? Token { get; private set; }
        public string? Email { get; private set; }
        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

        public event Action? Changed;

        public async Task LoadAsync(IJSRuntime js)
        {
            Token = await js.InvokeAsync<string?>("sessionStorage.getItem", "et_token");
            Email = await js.InvokeAsync<string?>("sessionStorage.getItem", "et_email");
            Changed?.Invoke();
        }

        public async Task SetAsync(IJSRuntime js, string token, string email)
        {
            Token = token;
            Email = email;
            await js.InvokeVoidAsync("sessionStorage.setItem", "et_token", token);
            await js.InvokeVoidAsync("sessionStorage.setItem", "et_email", email);
            Changed?.Invoke();
        }

        public async Task ClearAsync(IJSRuntime js)
        {
            Token = null;
            Email = null;
            await js.InvokeVoidAsync("sessionStorage.removeItem", "et_token");
            await js.InvokeVoidAsync("sessionStorage.removeItem", "et_email");
            Changed?.Invoke();
        }
    }
}

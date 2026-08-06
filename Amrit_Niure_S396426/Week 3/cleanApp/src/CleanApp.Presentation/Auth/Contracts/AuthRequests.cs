namespace CleanApp.Presentation.Auth.Contracts;

public sealed record RegisterRequest(string Email, string Password);

public sealed record LoginRequest(string Email, string Password);

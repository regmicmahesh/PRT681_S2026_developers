namespace authentication.Auth
{
    public static class LogoutUser
    {
        public record Request(string RefreshToken);

        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            // No RequireAuthorization here: the access token TTL is intentionally short (minutes),
            // so by the time a client logs out its access token may already be expired.
            // Possession of the (single-use, server-validated, hashed) refresh token is itself
            // the credential for this endpoint - the same trust model an OAuth revocation endpoint uses.
            app.MapPost("/logout", async (Request request, RefreshTokenService refreshTokenService) =>
            {
                await refreshTokenService.RevokeAsync(request.RefreshToken);
                return Results.NoContent();
            });
        }
    }
}

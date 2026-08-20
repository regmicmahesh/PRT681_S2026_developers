namespace JobBoard.Api.Authorization;

// Must match Auth.Authorization.CustomClaimTypes.Permission in the Auth service - this is the
// claim type its JWTs carry permission grants under.
public static class CustomClaimTypes
{
    public const string Permission = "permission";
}

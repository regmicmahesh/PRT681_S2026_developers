namespace JobBoard.Api.Authorization;

// Subset of Auth.Authorization.Permissions relevant to this service. Values must match the Auth
// service exactly - they're the contract by which its JWTs grant capabilities here. The Auth
// service issues these; this service only ever checks them, it never seeds or grants them.
public static class Permissions
{
    public const string JobCreate = "job:create";
    public const string JobRead = "job:read";
    public const string JobUpdate = "job:update";
    public const string JobDelete = "job:delete";
    public const string JobApply = "job:apply";

    // Admin-only override. See Auth service's Authorization/README.md for why job:update/job:delete
    // alone must never be used as the cross-company bypass.
    public const string JobManageAny = "job:manage-any";
}

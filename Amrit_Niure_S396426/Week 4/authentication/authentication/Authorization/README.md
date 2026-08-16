# Authorization conventions

This app uses **permission claims**, not roles, as the sole authorization signal. Roles
(`Roles.Admin`, `Roles.Member`) are only a grouping mechanism used at seed time to grant a set of
permission claims to a user via `SeedRolesAndPermissions` in [`Extensions.cs`](Extensions.cs) - they
are never checked directly when deciding whether a request is allowed.

**Do not call `RequireRole(...)` or branch on `ClaimTypes.Role` in new authorization code.**
`ClaimTypes.Role` claims exist in the JWT (see `Controllers/AuthController.cs`) purely for display -
e.g. the `/me` endpoint's `Roles` field. To gate a new endpoint, add or reuse a permission constant
in `Permissions.cs` and grant it to the relevant role in `SeedRolesAndPermissions`.

## Permission checks (`PermissionRequirement` / `PermissionAuthorizationHandler`)

- `RequireAnyPermission("a", "b")` - caller needs at least one of the listed permissions (OR).
- `RequireAllPermissions("a", "b")` - caller needs every listed permission (AND).
- `RequirePermission(...)` is a backward-compatible alias for `RequireAnyPermission`.

For controller actions that only need a plain (non-resource-based) permission check, register a
named policy once in `Program.cs` and reference it declaratively:

```csharp
options.AddPolicy("RequireUsersRead", policy => policy.RequireAnyPermission(Permissions.UsersRead));
```
```csharp
[HttpGet]
[Authorize(Policy = "RequireUsersRead")]
public async Task<IActionResult> GetAll() { ... }
```

## Resource ownership (`SameUserOrPermissionRequirement` / `SameUserOrPermissionAuthorizationHandler`)

For endpoints where a user should always be able to act on their own resource, but acting on
someone else's requires an explicit permission (e.g. `GET /users/{id}` and `PUT /users/{id}` in
`Controllers/UsersController.cs`), attributes can't help - `[Authorize(Policy = "...")]` has no way
to know what the `{id}` route value even is. So the action carries a plain `[Authorize]` (authenticated
users only) and does the real check explicitly in the body:

```csharp
var authResult = await authorizationService.AuthorizeAsync(
    User, id, new SameUserOrPermissionRequirement(Permissions.UsersUpdate));
if (!authResult.Succeeded)
    return Forbid();
```

This succeeds if `id` equals the caller's own id, or if the caller holds the given permission.

**Important**: the override permission must actually be scoped to the people who should get it.
`Member` is deliberately never granted `user:read`/`user:update` in `SeedRolesAndPermissions` - a
Member's access to their own record comes entirely from the ownership check, not a permission
claim. If `user:update` were handed out broadly (e.g. to every Member), the permission branch would
succeed for everyone and the ownership boundary would be a no-op in practice.

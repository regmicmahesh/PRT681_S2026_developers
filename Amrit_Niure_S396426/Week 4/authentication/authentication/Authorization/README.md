# Authorization conventions

This app uses **permission claims**, not roles, as the sole authorization signal. Roles
(`Roles.Admin`, `Roles.Member`) are only a grouping mechanism used at seed time to grant a set of
permission claims to a user via `SeedRolesAndPermissions` in [`Extensions.cs`](Extensions.cs) - they
are never checked directly when deciding whether a request is allowed.

**Do not call `RequireRole(...)` or branch on `ClaimTypes.Role` in new authorization code.**
`ClaimTypes.Role` claims exist in the JWT (see `Auth/LoginUser.cs`) purely for display - e.g. the
`/me` endpoint's `Roles` field. To gate a new endpoint, add or reuse a permission constant in
`Permissions.cs` and grant it to the relevant role in `SeedRolesAndPermissions`.

## Permission checks (`PermissionRequirement` / `PermissionAuthorizationHandler`)

- `RequireAnyPermission("a", "b")` - caller needs at least one of the listed permissions (OR).
- `RequireAllPermissions("a", "b")` - caller needs every listed permission (AND).
- `RequirePermission(...)` is a backward-compatible alias for `RequireAnyPermission`.

Used inline via `RequireAuthorization(policy => policy.RequireAnyPermission(...))`.

## Resource ownership (`SameUserOrPermissionRequirement` / `SameUserOrPermissionAuthorizationHandler`)

For endpoints where a user should always be able to act on their own resource, but acting on
someone else's requires an explicit permission (e.g. `GET /users/{id}` and `PUT /users/{id}` in
`Auth/UsersApi.cs`):

```csharp
var authResult = await authorizationService.AuthorizeAsync(
    principal, targetResourceId, new SameUserOrPermissionRequirement(Permissions.UsersUpdate));
```

This succeeds if `targetResourceId` equals the caller's own id, or if the caller holds the given
permission. Route-level `.RequireAuthorization()` only enforces authentication here - the explicit
`AuthorizeAsync` call inside the handler performs the actual resource-based decision.

**Important**: the override permission must actually be scoped to the people who should get it.
`Member` is deliberately never granted `user:read`/`user:update` in `SeedRolesAndPermissions` - a
Member's access to their own record comes entirely from the ownership check, not a permission
claim. If `user:update` were handed out broadly (e.g. to every Member), the permission branch would
succeed for everyone and the ownership boundary would be a no-op in practice.

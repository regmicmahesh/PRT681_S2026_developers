namespace authentication.Authorization
{
    public static class Permissions
    {
        // User management (enforced by this service).
        public const string UsersRead = "user:read";
        public const string UsersUpdate = "user:update";
        public const string UsersDelete = "user:delete";
        public const string UsersManageRoles = "user:manage-roles";

        // Job-board domain permissions. This auth service only issues them as claims in the JWT -
        // the services that own jobs/applications/candidates are responsible for enforcing them.
        public const string JobCreate = "job:create";
        public const string JobRead = "job:read";
        public const string JobUpdate = "job:update";
        public const string JobDelete = "job:delete";
        public const string JobApply = "job:apply";

        public const string ApplicationReadOwn = "application:read-own";
        public const string ApplicationReadAny = "application:read-any";
        public const string ApplicationManage = "application:manage";

        public const string CandidateSearch = "candidate:search";
    }
}


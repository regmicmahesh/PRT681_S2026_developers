namespace authentication.Authorization
{
    public static class Permissions
    {
        public const string UsersRead = "user:read";
        public const string UsersUpdate = "user:update";
        public const string UsersDelete = "user:delete";

        // Placeholders for actual job-board features - rename/extend as those are built.
        public const string JobApply = "job:apply";
        public const string JobView = "job:view";
    }
}


namespace Auth.Data
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string JobSeeker = "JobSeeker";
        public const string Employer = "Employer";
        public const string Recruiter = "Recruiter";

        public static readonly IReadOnlyCollection<string> All = [Admin, JobSeeker, Employer, Recruiter];

        // Roles a caller may pick for themselves at POST /register. Admin is never self-service -
        // it's provisioned by seeding or promoted later by an existing Admin via PUT /users/{id}/role.
        public static readonly IReadOnlyCollection<string> SelfRegisterable = [JobSeeker, Employer, Recruiter];
    }
}

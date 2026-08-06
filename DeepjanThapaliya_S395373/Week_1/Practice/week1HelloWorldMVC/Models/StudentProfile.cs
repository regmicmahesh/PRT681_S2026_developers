namespace week1HelloWorldMVC.Models;

public class StudentProfile
{
    public string StudentId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string UnitName { get; set; } = string.Empty;

    public int CurrentWeek { get; set; }

    public bool IsEnrolled { get; set; }
}
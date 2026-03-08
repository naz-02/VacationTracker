using SQLite;

namespace VacationTracker.Models;

public class Employee
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public decimal VacationBalance { get; set; } = 22;
    public DateTime RecruitmentDate { get; set; } = DateTime.Now;
    public string Department { get; set; } = string.Empty;
}

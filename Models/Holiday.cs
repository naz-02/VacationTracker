using SQLite;

namespace VacationTracker.Models;

public class Holiday
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsReligious { get; set; }
}

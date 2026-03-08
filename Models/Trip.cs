using SQLite;

namespace VacationTracker.Models;

public class Trip
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);
    public decimal Budget { get; set; }

    [Indexed]
    public int EmployeeId { get; set; }
    public string Status { get; set; } = "Pending";
    public string LeaveType { get; set; } = "Annuel"; // Annuel or Exceptionnel

    // [AGENT HOOK: Expand Model]
    // An agent can add properties here for external API data integration.
    // e.g., public string? WeatherForecastData { get; set; } 
    // e.g., public string? MapCoordinates { get; set; }

    // Ignored in SQLite DB directly to keep things simple. 
    // You would query activities separately using the TripId.
    [Ignore]
    public List<TripActivity> Activities { get; set; } = new();
}

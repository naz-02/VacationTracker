using SQLite;

namespace VacationTracker.Models;

public class TripActivity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Foreign Key mapping back to the Trip
    [Indexed]
    public int TripId { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime ActivityDate { get; set; }
    public decimal Cost { get; set; }
    
    // [AGENT HOOK: Expand Activity Model]
    // An agent can add location or booking data for activities.
    // e.g., public string? PlaceId { get; set; } // For Google Maps Integration
    // e.g., public bool IsBooked { get; set; }
}

using SQLite;
using VacationTracker.Models;

namespace VacationTracker.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        // Force the database location to a public folder to ensure full permissions
        _dbPath = "C:\\Users\\Public\\VacationTracker.db";
    }

    private List<DateTime> _cachedHolidays = new();

    public bool WasStaffSeededThisSession { get; private set; } = false;

    private async Task InitAsync()
    {
        if (_database is not null)
            return;

        try 
        {
            _database = new SQLiteAsyncConnection(_dbPath, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create);
            await _database.CreateTableAsync<Trip>();
        }
        catch (Exception)
        {
            string emergencyPath = Path.Combine(FileSystem.AppDataDirectory, "VacationTracker_Emergency.db");
            _database = new SQLiteAsyncConnection(emergencyPath, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create);
        }
        
        await _database.CreateTableAsync<Trip>();
        await _database.CreateTableAsync<TripActivity>();
        await _database.CreateTableAsync<Employee>();
        await _database.CreateTableAsync<Holiday>();

        // Initialize cache
        var holidaysByDb = await _database.Table<Holiday>().ToListAsync();
        _cachedHolidays = holidaysByDb.Select(h => h.Date.Date).ToList();        _cachedHolidays = holidaysByDb.Select(h => h.Date.Date).ToList();

        // ONE-TIME SESSION SETUP (20 Employees + 2026 Holidays)
        if (!WasStaffSeededThisSession) 
        {
            // --- 1. SEED 2026 HOLIDAYS ---
            await _database.DropTableAsync<Holiday>();
            await _database.CreateTableAsync<Holiday>();

            var seedHolidays = new List<Holiday>
            {
                new Holiday { Name = "New Year / رأس السنة", Date = new DateTime(2026, 1, 1), IsReligious = false },
                new Holiday { Name = "Independence Manifesto / تقديم وثيقة الاستقلال", Date = new DateTime(2026, 1, 11), IsReligious = false },
                new Holiday { Name = "Berber New Year / رأس السنة الأمازيغية", Date = new DateTime(2026, 1, 14), IsReligious = false },
                new Holiday { Name = "Aïd el-Fitr / عيد الفطر", Date = new DateTime(2026, 3, 20), IsReligious = true },
                new Holiday { Name = "Aïd el-Fitr / عيد الفطر (2)", Date = new DateTime(2026, 3, 21), IsReligious = true },
                new Holiday { Name = "Labor Day / عيد الشغل", Date = new DateTime(2026, 5, 1), IsReligious = false },
                new Holiday { Name = "Eid Al-Adha / عيد الأضحى", Date = new DateTime(2026, 5, 27), IsReligious = true },
                new Holiday { Name = "Eid Al-Adha / عيد الأضحى (2)", Date = new DateTime(2026, 5, 28), IsReligious = true },
                new Holiday { Name = "1er Moharram / فاتح محرم", Date = new DateTime(2026, 6, 17), IsReligious = true },
                new Holiday { Name = "Throne Day / عيد العرش", Date = new DateTime(2026, 7, 30), IsReligious = false },
                new Holiday { Name = "Oued Ed-Dahab / ذكرى استرجاع وادي الذهب", Date = new DateTime(2026, 8, 14), IsReligious = false },
                new Holiday { Name = "Revolution / ثورة الملك والشعب", Date = new DateTime(2026, 8, 20), IsReligious = false },
                new Holiday { Name = "Youth Day / عيد الشباب", Date = new DateTime(2026, 8, 21), IsReligious = false },
                new Holiday { Name = "Aïd el-Mouloud / المولد النبوي", Date = new DateTime(2026, 8, 26), IsReligious = true },
                new Holiday { Name = "Aïd el-Mouloud / المولد النبوي (2)", Date = new DateTime(2026, 8, 27), IsReligious = true },
                new Holiday { Name = "Green March / ذكرى المسيرة الخضراء", Date = new DateTime(2026, 11, 6), IsReligious = false },
                new Holiday { Name = "Independence Day / عيد الاستقلال", Date = new DateTime(2026, 11, 18), IsReligious = false }
            };
            await _database.InsertAllAsync(seedHolidays);

            // Update Cache immediately
            var hList = await _database.Table<Holiday>().ToListAsync();
            _cachedHolidays = hList.Select(h => h.Date.Date).ToList();

            // --- 2. SEED OFFICIAL STAFF ---
            await _database.DeleteAllAsync<Employee>();

            var officialStaff = new List<Employee>
            {
                new Employee { FullName = "Directeur Général / المدير العام", Role = "Directeur Général (Metasarrif Momtaz)", Department = "Direction Générale", RecruitmentDate = new DateTime(1994, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Employé(e) 01 / 01 موظف", Role = "Metasarrif Momtaz", Department = "Présidence et Conseil", RecruitmentDate = new DateTime(1994, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Employé(e) 02 / 02 موظف", Role = "Metasarrif 1er Grade", Department = "Présidence et Conseil", RecruitmentDate = new DateTime(1989, 9, 1), VacationBalance = 22 },
                new Employee { FullName = "Ingénieur en Chef 1 / 1 مهندس رئيس", Role = "Ingénieur en Chef", Department = "Service Administratif & RH", RecruitmentDate = new DateTime(1999, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Ingénieur en Chef 2 / 2 مهندس رئيس", Role = "Ingénieur en Chef", Department = "Service Technique", RecruitmentDate = new DateTime(1999, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Employé(e) 03 / 03 موظف", Role = "Rédacteur 1er Grade", Department = "Service Administratif", RecruitmentDate = new DateTime(1994, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Employé(e) 04 / 04 موظف", Role = "Rédacteur 1er Grade", Department = "Présidence et Conseil", RecruitmentDate = new DateTime(1994, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Employé(e) 05 / 05 موظف", Role = "Rédacteur 1er Grade", Department = "Affaires Financières", RecruitmentDate = new DateTime(2002, 6, 25), VacationBalance = 22 },
                new Employee { FullName = "Employé(e) 06 / 06 موظف", Role = "Rédacteur 1er Grade", Department = "Service Administratif & RH", RecruitmentDate = new DateTime(1995, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 01 / 01 تقني", Role = "Technicien 1er Grade", Department = "Service Administratif & RH", RecruitmentDate = new DateTime(1987, 9, 1), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 02 / 02 تقني", Role = "Technicien 1er Grade", Department = "Affaires Financières", RecruitmentDate = new DateTime(1993, 1, 4), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 03 / 03 تقني", Role = "Technicien 1er Grade", Department = "Affaires Financières", RecruitmentDate = new DateTime(2000, 7, 1), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 04 / 04 تقني", Role = "Technicien 1er Grade", Department = "Affaires Financières", RecruitmentDate = new DateTime(1999, 11, 1), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 05 / 05 تقني", Role = "Technicien 2ème Grade", Department = "Service Technique", RecruitmentDate = new DateTime(2005, 3, 15), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 06 / 06 تقني", Role = "Technicien 2ème Grade", Department = "Service Technique", RecruitmentDate = new DateTime(2015, 6, 15), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 07 / 07 تقني", Role = "Technicien 2ème Grade", Department = "Service Technique", RecruitmentDate = new DateTime(2010, 5, 20), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 08 / 08 تقني", Role = "Technicien 2ème Grade", Department = "Service Technique", RecruitmentDate = new DateTime(2012, 8, 10), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 09 / 09 تقني", Role = "Technicien 3ème Grade", Department = "Service Administratif", RecruitmentDate = new DateTime(2016, 1, 1), VacationBalance = 22 },
                new Employee { FullName = "Technicien(ne) 10 / 10 تقني", Role = "Technicien 3ème Grade", Department = "Service Technique", RecruitmentDate = new DateTime(2023, 10, 2), VacationBalance = 22 },
                new Employee { FullName = "Employé(e) 07 / 07 موظف", Role = "Adjoint Technique Momtaza", Department = "Service Administratif", RecruitmentDate = new DateTime(1991, 8, 1), VacationBalance = 22 }
            };
            await _database.InsertAllAsync(officialStaff);
            
            WasStaffSeededThisSession = true;
        }
    }

    public async Task<List<Trip>> GetTripsAsync()
    {
        await InitAsync();
        return await _database!.Table<Trip>().ToListAsync();
    }

    public async Task<int> AddTripAsync(Trip trip)
    {
        await InitAsync();
        return await _database!.InsertAsync(trip);
    }

    public async Task<int> DeleteTripAsync(Trip trip)
    {
        await InitAsync();
        return await _database!.DeleteAsync(trip);
    }
    
    public async Task<List<TripActivity>> GetActivitiesForTripAsync(int tripId)
    {
        await InitAsync();
        return await _database!.Table<TripActivity>()
            .Where(a => a.TripId == tripId)
            .OrderBy(a => a.ActivityDate)
            .ToListAsync();
    }

    public async Task<int> AddTripActivityAsync(TripActivity activity)
    {
        await InitAsync();
        return await _database!.InsertAsync(activity);
    }
    
    public async Task<Trip?> GetTripByIdAsync(int id)
    {
        await InitAsync();
        return await _database!.Table<Trip>().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        await InitAsync();
        return await _database!.Table<Employee>().ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        await InitAsync();
        return await _database!.Table<Employee>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<int> UpdateEmployeeBalanceAsync(int employeeId, decimal daysToSubtract)
    {
        await InitAsync();
        var employee = await GetEmployeeByIdAsync(employeeId);
        if (employee != null)
        {
            employee.VacationBalance -= daysToSubtract;
            return await _database!.UpdateAsync(employee);
        }
        return 0;
    }

    public async Task<int> ApproveTripAsync(int tripId)
    {
        await InitAsync();
        var trip = await GetTripByIdAsync(tripId);
        if (trip != null && trip.Status != "Approved")
        {
            if (trip.LeaveType == "Annuel")
            {
                var days = CalculateWorkingDays(trip.StartDate, trip.EndDate);
                await UpdateEmployeeBalanceAsync(trip.EmployeeId, (decimal)days);
            }
            trip.Status = "Approved";
            return await _database!.UpdateAsync(trip);
        }
        return 0;
    }

    public async Task<int> RejectTripAsync(int tripId)
    {
        await InitAsync();
        var trip = await GetTripByIdAsync(tripId);
        if (trip != null)
        {
            trip.Status = "Rejected";
            return await _database!.UpdateAsync(trip);
        }
        return 0;
    }

    public async Task<List<Holiday>> GetHolidaysAsync()
    {
        await InitAsync();
        return await _database!.Table<Holiday>().OrderBy(h => h.Date).ToListAsync();
    }

    public async Task<int> UpdateHolidayAsync(Holiday holiday)
    {
        await InitAsync();
        var result = await _database!.UpdateAsync(holiday);
        
        // Refresh cache
        var holidays = await _database.Table<Holiday>().ToListAsync();
        _cachedHolidays = holidays.Select(h => h.Date.Date).ToList();
        
        return result;
    }

    public int CalculateWorkingDays(DateTime start, DateTime end)
    {
        // Use cached holidays to avoid async blocking
        var holidaySet = _cachedHolidays.ToHashSet();

        int workingDays = 0;
        for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !holidaySet.Contains(date))
            {
                workingDays++;
            }
        }
        return workingDays;
    }
}

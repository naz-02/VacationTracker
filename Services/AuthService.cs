namespace VacationTracker.Services;

public class AuthService
{
    public bool IsAuthenticated { get; private set; } = false;

    public bool Login(string username, string password)
    {
        if (username == "Nassro" && password == "Naz2020")
        {
            IsAuthenticated = true;
            return true;
        }
        return false;
    }

    public void Logout()
    {
        IsAuthenticated = false;
    }
}

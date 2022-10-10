using Models;
using DataAccess;

namespace Services;

public class Authentication
{
    private bool _authenticated;
    public Authentication() {
        _authenticated = false;
        //StaticStorage.GetFilesInfo();
    }

    public bool UsernameExists(string username) {
        if (StaticStorage.CheckUsername(username)) {
            return true;
        }
        else    return false;
    }

    public bool Register(string username, string password) {
        if (!UsernameExists(username)) {
            StaticStorage.AddLogIn(username, password);
            StaticStorage.AddEmployee(username);
            return true;
        }
        _authenticated = false;
        return false;
    }

    public bool LogIn(string username, string password) {
        if (UsernameExists(username)) {
            if (StaticStorage.VerifyCredentials(username, password)) {

                if (StaticStorage.IsManager(username))  ERSService.ManagerLogIn(username);
                else    ERSService.EmployeeLogIn(username);

                _authenticated = true;
                return true;
            }
        }
        _authenticated = false;
        return false;
    }

    public void Exiting() {
        StaticStorage.UpdateFiles();
        return;
    }
}

public class ERSService
{
    private static Employee? eUser = null;
    private static Manager? mUser = null;

    public static void EmployeeLogIn(string username) {
        eUser = StaticStorage.GetEmployee(username);
        return;
    }
    public static void ManagerLogIn(string username) {
        mUser = StaticStorage.GetManager(username);
        return;
    }

    public static bool ManagerCheck() {
        if (mUser != null)  return true;
        else    return false;
    }
    public static string AccountInfo() {
        return (eUser ?? mUser).Info();
    }

    public static List<Ticket>? GetTickets() {
        if ((eUser ?? mUser).tickets.Count > 0)  return (eUser ?? mUser).tickets;
        else    return null;
    }
    public static void AddTicket(Ticket t) {
        (eUser ?? mUser).tickets.Add(t);
        StaticStorage.NewTicket(t);
        return;
    }

    public static Ticket? TicketFromQueue() {
        if (StaticStorage.QueueLength() > 0)    return StaticStorage.GetFromQueue();
        else    return null;
    }
    public static void ReviewTicket(Ticket t, bool choice) {
        t.Review(mUser, choice);
        return;
    }
}

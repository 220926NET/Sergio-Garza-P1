using Models;
using DataAccess;

namespace Services;

public class Authentication
{
    private IERSStorage _repo;
    private bool _authenticated;

    public Authentication() {
        _authenticated = false;
        _repo = new DBRepo();
    }

    public bool UsernameExists(string username) {
        if (_repo.CheckUsername(username)) return true;
        else    return false;
    }

    public bool Register(string username, string password) {
        if (!UsernameExists(username)) {
            _repo.AddLogIn(username, password);
            _repo.AddEmployee(username);
            return true;
        }
        _authenticated = false;
        return false;
    }

    public ERSService? LogIn(string username, string password) {
        if (UsernameExists(username)) {
            if (_repo.VerifyCredentials(username, password)) {
                ERSService service = new ERSService(_repo, username);

                _authenticated = true;
                return service;
            }
        }
        _authenticated = false;
        return null;
    }

    public void Exiting() {
        _authenticated = false;
        return;
    }
}

public class ERSService
{
    private Employee? eUser = null;
    private Manager? mUser = null;
    private IERSStorage _repo;

    public ERSService(IERSStorage repo, string username) {
        _repo = repo;

        if (_repo.IsManager(username))  ManagerLogIn(username);
        else    EmployeeLogIn(username);
    }

    private void EmployeeLogIn(string username) {
        eUser = _repo.GetEmployee(username);
        return;
    }
    private void ManagerLogIn(string username) {
        mUser = _repo.GetManager(username);
        return;
    }

    public bool ManagerCheck() {
        if (mUser != null)  return true;
        else    return false;
    }
    public string AccountInfo() {
        return (eUser ?? mUser).Info();
    }

    public List<Ticket>? GetTickets() {
        if (_repo.TicketsCount((eUser ?? mUser).username) > 0)  return _repo.GetTickets((eUser ?? mUser).username);
        else    return null;
    }
    public void AddTicket(Ticket t) {
        _repo.NewTicket(t, (eUser ?? mUser).username);
        return;
    }

    public Ticket? TicketFromQueue() {
        if (_repo.QueueLength() > 0)    return _repo.GetFromQueue();
        else    return null;
    }
    public void ReviewTicket(Ticket t, bool choice) {
        t.Review(mUser, choice);
        return;
    }
}

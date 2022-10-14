using Models;
using DataAccess;

namespace Services;

public class Authentication
{
    private IERSStorage _repo;
    private bool _authenticated;

    public Authentication() {
        _authenticated = false;
        _repo = new DBRepo();           // FOR dependency injection, COULD switch out with initialized factory??
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

    public EmployeeService? LogIn(string username, string password) {
        if (UsernameExists(username)) {
            if (_repo.VerifyCredentials(username, password)) {
                EmployeeService service = new EmployeeService(_repo, username);

                _authenticated = true;
                return service;
            }
        }
        _authenticated = false;
        return null;
    }

    public void Exiting() {                 // NECESSARY or not?? Maybe if implementing Unit of Work Pattern?
        _authenticated = false;
        return;
    }
}

public class EmployeeService
{
    private IERSStorage _repo;
    private Employee? eUser = null;
    private Manager? mUser = null;

    public EmployeeService(IERSStorage repo, string username) {
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
    public void AddTicket(decimal amount, string description) {
        _repo.NewTicket(new Ticket((eUser ?? mUser).username, amount, description));
        return;
    }


    public Ticket? TicketFromQueue() {
        return _repo.GetFromQueue();
    }
    public void ReviewTicket(Ticket t, bool choice) {
        t.Review(mUser, choice);
        _repo.UpdateStatus(t);
        return;
    }
}
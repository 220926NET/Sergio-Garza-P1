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
        else    return false;
    }

    public EmployeeService? LogIn(string username, string password) {
        if (UsernameExists(username)) {
            if (_repo.VerifyCredentials(username, password)) {
                EmployeeService service;

                if (_repo.IsManager(username))  
                    service = new ManagerService(_repo, username);
                else    
                    service = new EmployeeService(_repo, username);

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
    protected IERSStorage _repo;
    protected string _username;

    public EmployeeService(IERSStorage repo, string username) {
        _repo = repo;
        _username = username; 
    }

    public List<Ticket>? GetTickets() {
        if (_repo.TicketsCount(_username) > 0)  
            return _repo.GetTickets(_username);
        else    return null;
    }
    public void AddTicket(decimal amount, string description) {
        _repo.NewTicket(new Ticket(_username, amount, description));
        return;
    }

    public virtual string? AccountInfo() {
        Employee? user = _repo.GetEmployee(_username);
        if (user == null)   return null;
        else    return user.Info();
    }

    public virtual bool ManagerCheck() {    return false;   }
    public virtual Ticket? TicketFromQueue() {  return null;    }
    public virtual void ReviewTicket(Ticket t, bool choice) {   return; }
    public virtual bool? RoleCheck(string username) {    return null; }
    public virtual void ChangeRole(string username) {   return; }
}

public class ManagerService : EmployeeService
{
    public ManagerService(IERSStorage repo, string username) : base(repo, username) {}

    public override bool ManagerCheck() {    return true;    }

    public override string? AccountInfo() {
        Manager? user = _repo.GetManager(_username);
        if (user == null)   return null;
        else    return user.Info();
    }

    public override Ticket? TicketFromQueue() {
        return _repo.GetFromQueue();
    }
    public override void ReviewTicket(Ticket t, bool choice) {
        t.Review(_repo.GetManager(_username), choice);
        _repo.UpdateStatus(t);
        return;
    }

    public override bool? RoleCheck(string username) {
        if (_repo.CheckUsername(username)) {
            if (_repo.IsManager(username))  return true;
            else    return false;
        }
        else    return null;
    }
    public override void ChangeRole(string username) {
        if (_repo.IsManager(username)) {
            // delete from manager talbe
        }
        else {
            // add to manager table
        }
        return;
    }
}
using Models;
using DataAccess;

namespace Services;

public class Authentication
{
    private IERSStorage _repo;

    public Authentication() {
        _repo = new DBRepo();           // FOR dependency injection, COULD switch out with initialized factory??
    }

    public bool Register(string username, string password) {
        if (!_repo.UsernameExists(username)) {
            _repo.AddLogIn(username, password);
            _repo.AddEmployee(username);
            return true;
        }
        else    return false;
    }

    public int LogIn(string username, string password) {
        if (_repo.UsernameExists(username)) {
            if (_repo.VerifyCredentials(username, password)) {
                return _repo.UserId(username);
            }
        }
        return 0;
    }

    public void Exiting() {                 // NECESSARY or not?? Maybe if implementing Unit of Work Pattern?
        // LoggedIn bit to false in DB!!
        return;
    }
}

public class ERSService
{
    private IERSStorage _repo;

    public ERSService() {
        _repo = new DBRepo();           // FOR dependency injection, COULD switch out with initialized factory??
    }

    public int AddTicket(int id, decimal amount, string description) {
        return _repo.NewTicket(new Ticket(id, amount, description));
    }

    public List<Ticket>? Tickets(int? userId) { 
        return _repo.GetTickets(userId);
    }

    public bool ManagerCheck(int empId) {
        return _repo.IsManager(empId);
    }

    public void ReviewTicket(int tNum, int reviewer, string choice) {
        //_repo.UpdateStatus(tNum, reviewer, (choice ? "Approved" : "Denied"));
        _repo.UpdateStatus(tNum, reviewer, choice);
        return;
    }

    // public string? AccountInfo() {
    //     Employee? user = _repo.GetEmployee(_username);
    //     if (user == null)   return null;
    //     else    return user.Info();
    // }

    // public string? AccountInfo() {
    //     Manager? user = _repo.GetManager(_username);
    //     if (user == null)   return null;
    //     else    return user.Info();
    // }

    // public Ticket? TicketFromQueue() {
    //     return _repo.GetFromQueue();
    // }

    // public bool? RoleCheck(string username) {
    //     if (_repo.UsernameExists(username)) {
    //         if (_repo.IsManager(username))  return true;
    //         else    return false;
    //     }
    //     else    return null;
    // }
    // public void ChangeRole(string username) {
    //     if (_repo.IsManager(username)) {
    //         // delete from manager talbe
    //     }
    //     else {
    //         // add to manager table
    //     }
    //     return;
    // }
}
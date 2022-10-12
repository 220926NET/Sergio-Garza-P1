using Models;

namespace DataAccess;

public interface IERSStorage
{
    Employee? GetEmployee(string username);
    Manager? GetManager(string username);
    List<Ticket>? GetTickets(string username);
    Ticket GetFromQueue();

    void AddLogIn(string username, string password);
    void AddEmployee(string username);
    void NewTicket(Ticket t, string username);

    bool CheckUsername(string username);
    bool VerifyCredentials(string username, string password);
    bool IsManager(string username);

    int TicketsCount(string username);
    int QueueLength();
}
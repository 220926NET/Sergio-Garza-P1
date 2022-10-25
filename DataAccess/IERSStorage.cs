using Models;

namespace DataAccess;

public interface IERSStorage
{
    // Data Retrieval
    // Employee? GetEmployee(string username); // Will use username to return user's data in Employee object, returns null if user doesn't exist (or exception)
    // Manager? GetManager(string username);   // Will use username to return user's data in Manager object, returns null if user doesn't exist (or exception)
    //List<Ticket>? GetTickets(string username);  // Will use username to return user's ticket(s) data in List, returns null if user has no tickets (or exception)
    List<Ticket>? GetTickets(int? userId);
    //Ticket? GetFromQueue();  // Will return the Ticket that is next in line of queue (FIFO), null if no tickets need reviewing

    // Data Appending
    void AddLogIn(string username, string password);    // Will append the new username and password to LogIns data
    void AddEmployee(string username);  // Will append the new username to Employee data
    //void NewTicket(Ticket t);  // Will append the data from Ticket object with username as user reference to Tickets data

    // Data Updates
    //void UpdateStatus(Ticket t);    // Will update the status of a ticket on database
    void UpdateStatus(int ticketNum, int reviewerId, string decision);

    // Data Checks
    bool UsernameExists(string username);    // Will return true/false depending on if username exists within LogIns data
    bool VerifyCredentials(string username, string password);   // Will return true/false depending on if username & password match exist as a pair in LogIns data
    bool IsManager(int id);    // Will return true/false depending on if username exists within Managers data

    // Data Counting
    //int TicketsCount(string username);  // Will return the amount of tickets associated to user by username in Tickets data
    //int QueueLength();  // Will return the amount of tickets in the Queue (pending tickets)

    int UserId(string username);
    int NewTicket(Ticket t);
}
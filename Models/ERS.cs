namespace Models;

public class Ticket
{
    // public decimal Amount {                         // SHOULD I add input validation at class level?? 
    //     get {   return Amount;   } 
    //     private set {
    //         if (Amount < 0) throw new ArgumentException("Invalid expense amount");
    //         else    Amount = value;
    //     }
    // }
    public string User { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }

    public string Status { get; private set; } = "pending";     // SHOULD these be fully private?? do outside sources need to access them directly?
    public string? Reviewer { get; private set; } = null;

    public Ticket(string user, decimal amount, string description)
    {
        User = user;
        Amount = amount;
        Description = description;
    }
    public Ticket(string user, decimal amount, string description, string status) {
        User = user;
        Amount = amount;
        Description = description;
        Status = status;
    }

    public void Review(Manager? m, bool approval) {
        if (m == null)  return;

        Reviewer = m.username;
        if (approval)  Status = "approved";
        else    Status = "denied";

        return;
    }

    public string Info() {
        return ($"Amount: {Amount}\nDescription: {Description}\n");
    }
}

public class Employee
{
    public string firstName, lastName, username;
    public string? middleName = null;

    //public Employee() {}
    public Employee(string first, string last, string user) {
        firstName = first;
        lastName = last;
        username = user;
    }
    public Employee(string first, string middle, string last, string user) {
        firstName = first;
        middleName = middle;
        lastName = last;
        username = user;
    }

    public string Info() {
        return ($"First Name: {firstName}, Middle Name: {(middleName != null ? middleName : "N/A")}, Last Name: {lastName}");
    }
}

public class Manager : Employee
{
    public Manager(string first, string last, string user) : base(first, last, user) {
        // firstName = first;
        // lastName = last;
        // username = user;
    }

    public Manager(string first, string middle, string last, string user) : base(first, middle, last, user) {}
}
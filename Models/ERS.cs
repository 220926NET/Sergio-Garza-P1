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
    public int? Id { get; private set; } = null;
    public int User { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }

    public string Status { get; private set; } = "pending";     // SHOULD these be fully private?? do outside sources need to access them directly?
    public int? Reviewer { get; private set; } = null;

    public Ticket(int user, decimal amount, string description)
    {
        User = user;
        Amount = amount;
        Description = description;
    }
    public Ticket(int user, decimal amount, string description, string status) {
        User = user;
        Amount = amount;
        Description = description;
        Status = status;
    }
    public Ticket(int num, int user, decimal amount, string description, string status) {
        Id = num;
        User = user;
        Amount = amount;
        Description = description;
        Status = status;
    }
    public Ticket(int num, int user, decimal amount, string description, string status, int reviewer) {
        Id = num;
        User = user;
        Amount = amount;
        Description = description;
        Status = status;
        Reviewer = reviewer;
    }

    public void Review(Manager? m, bool approval) {
        if (m == null)  return;

        Reviewer = m.userId;
        if (approval)  Status = "approved";
        else    Status = "denied";

        return;
    }

    public string Info() {
        return ($"Amount: ${Amount.ToString("F")}\nDescription: {Description}\nStatus: {Status}\n");
    }
}

public class Employee
{
    public int userId;
    protected string? firstName, lastName, middleName = null;

    //public Employee() {}
    public Employee(string first, string last, int user) {
        firstName = first;
        lastName = last;
        userId = user;
    }
    public Employee(string first, string middle, string last, int user) {
        firstName = first;
        middleName = middle;
        lastName = last;
        userId = user;
    }

    public string Info() {
        return ($"First Name: {(firstName != null ? firstName : "N/A")}, Middle Name: {(middleName != null ? middleName : "N/A")}, Last Name: {(lastName != null ? lastName : "N/A")}");
    }
}

public class Manager : Employee
{
    public Manager(string first, string last, int user) : base(first, last, user) {}
    public Manager(string first, string middle, string last, int user) : base(first, middle, last, user) {}
}
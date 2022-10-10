namespace Models;

public class Ticket
{
    private decimal _amount;
    private string _description;
    public bool Reviewed { get; private set; } = false;

    public bool Approved { get; private set; } = false;
    private string _reviewer;

    public Ticket(decimal amount, string description)
    {
        _amount = amount;
        _description = description;
        _reviewer = "";
    }

    public string Info() {
        return ($"Amount: {_amount}\nDescription: {_description}\n");
    }

    public void Review(Manager? m, bool approving) {
        if (m == null)  return;

        _reviewer = m.Name();
        Reviewed = true;

        if (approving)  Approved = true;
        return;
    }
}

public class Employee
{
    public string firstName;
    public string middleName = "";
    public string lastName;
    public List<Ticket> tickets = new List<Ticket>();

    public Employee(){
        firstName = "";
        lastName = "";
    }
    public Employee(string first, string last) {
        firstName = first;
        lastName = last;
    }
    public Employee(string first, string middle, string last) {
        firstName = first;
        middleName = middle;
        lastName = last;
    }

    // public bool HasTickets() {
    //     if (tickets.Count > 0) return true;
    //     else    return false;
    // }

    public string Name() {
        return firstName + " " + lastName;
    }
    public string Info() {
        return ($"First Name: {firstName}, Middle Name: {(middleName != "" ? middleName : "N/A")}, Last Name: {lastName}");
    }
}

public class Manager : Employee
{
    public Manager(string first, string last) {
        firstName = first;
        lastName = last;
    }
}
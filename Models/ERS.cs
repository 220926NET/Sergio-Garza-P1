namespace Models;
public class Ticket
{

}

public class Employee
{
    public string firstName;
    public string middleName = "";
    public string lastName;
    protected List<Ticket> _tickets = new List<Ticket>();

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
}

public class Manager : Employee
{
    public Manager(string first, string last) {
        firstName = first;
        lastName = last;
    }
}
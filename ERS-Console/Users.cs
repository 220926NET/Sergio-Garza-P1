class Employee 
{
    public string firstName;
    public string lastName;
    private List<Ticket> _tickets = new List<Ticket>();

    public Employee()
    {
        firstName = Inputs.realInput("First Name: ");
        lastName = Inputs.realInput("Last Name: ");
    }

    public void createTicket() {
        _tickets.Add(new Ticket());
    }

    public void viewTickets() {
        foreach (Ticket t in _tickets) {
            Console.WriteLine("Ticket Number: " + (_tickets.IndexOf(t) + 1));
            Console.Write(t.printInfo());
        }
    }
}

class Manager : Employee
{
    
}
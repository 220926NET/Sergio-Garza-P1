using Models;
using Services;

public class AuthenticationMenu
{
    public ERSService? Service { get; private set; } = null;

    private Authentication accessability; 
    public AuthenticationMenu() {
        accessability = new Authentication();
    }

    public void AccessMenu() {
        Console.WriteLine("Welcome to the company's expense reimbursement system!");

        if (Inputs.YesNoExit("Do you have a login?")) LogInMenu();
        else {
            if (Inputs.YesNoExit("Would you like to register?")) RegistrationMenu();
        }

        return;
    }

    private void RegistrationMenu() {
        Console.WriteLine("New User Registration");

        string username = Inputs.RealInput("New Username: ");
        while (accessability.UsernameExists(username)) {
            Console.WriteLine("Username already exists!");

            username = Inputs.RealInput("New Username: ");
        }
        
        string password = Inputs.RealInput("New Password: (must be at least 6 characters long)");
        while (password.Length < 6) {
            Console.WriteLine("Invalid Input - Too Short");

            password = Inputs.RealInput("New Password: (must be at least 6 characters long)");
        }

        if (accessability.Register(username, password)) {
            Console.WriteLine("New Account Registered!");
            Console.WriteLine("Please log in using registered credentials");

            LogInMenu();
            return;
        }
        else {
            Console.WriteLine("Registration Error");
            return;
        }
    }

    private void LogInMenu() {
        for (int a = 0; a < 4; a++) {
            string username = Inputs.RealInput("Username: (\"new\" to register)");

            if (username.ToLower() == "new") {
                RegistrationMenu();
                return;
            }

            string password = Inputs.RealInput("Password: ");
            Service = accessability.LogIn(username, password);

            if (Service != null)    return;

            Console.WriteLine($"Unrecognized Credentials - {3 - a} attempts remain");
        }
        return;
    }

    public void End() {
        if (Service != null) accessability.Exiting();
        return;
    }
}

public class UserMenu
{
    protected ERSService _service;

    public UserMenu(ERSService service) {
        _service = service;
    }

    protected void Menu() {
        Console.WriteLine("What would you like to do today?");
        Console.WriteLine("[0] Log Out");
        Console.WriteLine("[1] Create Ticket");             //MVP!!!
        Console.WriteLine("[2] View Your Previous Tickets");     //MVP!!!
        Console.WriteLine("[3] Review/Update Account Info");
        return;
    }

    protected int Selection() {
        do {
            string input = Inputs.RealInput("");
            bool number = int.TryParse(input, out int choice);

            if (number) {
                switch(choice) {
                    case 0:
                        return 0;
                    case 1:
                        CreateTicket();
                        return 1;
                    case 2:
                        ViewTickets();
                        return 2;
                    case 3:
                        ViewAccount();
                        UpdateAccount();
                        return 3;
                    case 4:
                        return 4;
                    case 5:
                        return 5;
                    default:
                        break;
                }
            }
                
            Console.WriteLine("Unrecognized Input (\"0\" to exit)");
        } while (true);
    }

    public void Repeat() {
        do {
            Menu();
            int e = Selection();

            if (e == 0)   break;
        } while (true);

        return;
    }

    protected void ViewTickets() {
        List<Ticket>? tickets = _service.GetTickets();

        if (tickets == null) {
            Console.WriteLine("You have not submitted any tickets");
            return;
        }

        foreach (Ticket t in tickets) {
            //Console.WriteLine("\nTicket Number: " + (tickets.IndexOf(t) + 1));
            Console.Write(t.Info());
        }
        return;
    }

    protected void CreateTicket() {
        string input = Inputs.RealInput("How much was the expense? (Numbers Only))");
        bool isNumber = decimal.TryParse(input, out decimal amount);

        while(!isNumber) {
            Console.WriteLine("Invalid Input");

            input = Inputs.RealInput("How much was the expense? (Numbers Only))");
            isNumber = decimal.TryParse(input, out amount);
        }

        string description = Inputs.RealInput("Description: ");

        _service.AddTicket(new Ticket(amount, description));
        return;
    }

    protected void ViewAccount() {
        Console.WriteLine(":Account Info:");
        Console.WriteLine(_service.AccountInfo());
        return;
    }

    protected void UpdateAccount() {
        if (Inputs.YesNoExit("Whould you like to update your account information?")) {

        }
        return;
    }
}

public class ManagerMenu : UserMenu
{   
    public ManagerMenu(ERSService service) : base(service) {
        //_service = service;
        Repeat();
    }

    private void ExtraOptions() {
        Console.WriteLine("[4] Process Tickets From Queue");    //MVP!!!
        Console.WriteLine("[5] Change an Employee's Role");
        return;
    }
    private new void Repeat() {
        do {
            Menu();
            ExtraOptions();

            int choice = Selection();

            if (choice == 0)   
                break;
            else if (choice == 4) 
                ProcessTickets();
            else if (choice == 5) {

            }
        } while (true);

        return;
    }

    private void ProcessTickets() {
        do {
            Ticket? t = _service.TicketFromQueue();
            if (t == null) {
                Console.WriteLine("There are no unreviewed tickets");
                return;
            }

            Console.WriteLine("Reviewing oldest ticket submission");
            Console.WriteLine("\nTicket Info: ");

            Console.Write(t.Info());

            _service.ReviewTicket(t, Inputs.YesNoExit("Approve ticket?"));

            if (!Inputs.YesNoExit("Would you like to continue with another ticket?"))   return;
        } while (true);
    }
}

public static class Inputs
{
    public static string RealInput(string msg) {
        do {
            if (msg != "")  Console.WriteLine(msg);
            string input = Console.ReadLine()!;

            if (!String.IsNullOrWhiteSpace(input)) {
                return input;
            }
        } while (true);
    }

    public static bool YesNoExit(string question) {
        do {
            string input = Inputs.RealInput(question + " (y/N)");
            bool letter = char.TryParse(input, out char choice);

            if (letter) {
                switch(char.ToLower(choice)) {
                    case 'n':
                    return false;
                    case 'y':
                    return true;
                    case 'e':
                    Environment.Exit(0);
                    return false;
                    default:
                    break;
                }
            }
                
            Console.WriteLine("Unrecognized Input (\"E\" to exit)");
        } while (true);
    }

    // public static int Number(string msg) {}
}
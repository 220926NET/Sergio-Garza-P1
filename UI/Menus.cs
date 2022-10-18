using Models;
using Services;

namespace Menus;

public class AuthenticationMenu
{
    public EmployeeService? Service { get; private set; } = null;

    private Authentication accessability; 
    public AuthenticationMenu() {
        accessability = new Authentication();
    }

    public void AccessMenu() {
        Console.WriteLine("------------------------------------------------------\nWelcome to the company's expense reimbursement system!\n------------------------------------------------------\n");

        if (Inputs.YesNoExit("Do you have a login?")) LogInMenu();
        else {
            if (Inputs.YesNoExit("Would you like to register?")) RegistrationMenu();
        }

        return;
    }

    private void RegistrationMenu() {
        Console.WriteLine("\n----------------------\nNew User Registration\n----------------------\n");

        string username = Inputs.RealInput("New Username: ");
        while (accessability.UsernameExists(username)) {
            Console.WriteLine("\n***Username already exists!***\n");

            username = Inputs.RealInput("New Username: ");
        }
        
        string password = Inputs.RealInput("New Password: (must be at least 6 characters long)");
        while (password.Length < 6) {       // SHOULD password validation be in Services(Authentication)??
            Console.WriteLine("\n***Invalid Password - Too Short***\n");

            password = Inputs.RealInput("New Password: (must be at least 6 characters long)");
        }

        if (accessability.Register(username, password)) {
            Console.Clear();
            Console.WriteLine("------------------------\nNew Account Registered!\n------------------------\n");
            Console.WriteLine("Please log in using registered credentials");

            LogInMenu();
            return;
        }
        else {
            Console.WriteLine("\n******************\nRegistration Error\n******************\n");
            return;
        }
    }

    private void LogInMenu() {
        for (int a = 0; a < 4; a++) {       // SHOULD attempt counter be in Services(Authentication)??
            string username = Inputs.RealInput("\nUsername: (\"new\" to register)");

            if (username.ToLower() == "new") {
                RegistrationMenu();
                return;
            }

            string password = Inputs.RealInput("Password: ");
            Service = accessability.LogIn(username, password);

            Console.Clear();
            if (Service != null)    return;

            Console.WriteLine($"Unrecognized Credentials - {3 - a} attempts remain");
        }
        return;
    }

    public void End() {                 // NECESSARY or not??
        if (Service != null) accessability.Exiting();
        return;
    }
}

public class UserMenu
{
    protected EmployeeService _service;

    public UserMenu(EmployeeService service) {
        _service = service;
    }

    protected void Menu() {
        Console.WriteLine("\nWhat would you like to do?");
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

            Console.Clear();

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
                
            Console.WriteLine("\n***Unrecognized Input (\"0\" to exit)***\n");
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
            Console.WriteLine("You have not submitted any tickets.\n");
            return;
        }

        foreach (Ticket t in tickets) {
            //Console.WriteLine("\nTicket Number: " + (tickets.IndexOf(t) + 1));
            Console.WriteLine(t.Info() + "------------------------------------");
        }
        return;
    }

    protected void CreateTicket() {
        string input = Inputs.RealInput("How much was the expense? (Numbers Only)");
        bool isNumber = decimal.TryParse(input, out decimal amount);

        while(!isNumber) {                  // COULD put the above two commands into each other, then in place of isNumber as condition??
            Console.WriteLine("\n***Invalid Input***\n");

            input = Inputs.RealInput("How much was the expense? (Numbers Only)");
            isNumber = decimal.TryParse(input, out amount);
        }

        string description = Inputs.RealInput("Description: ");

        _service.AddTicket(amount, description);
        return;
    }

    protected void ViewAccount() {
        Console.WriteLine("------------\nAccount Info\n------------\n");
        string? info = _service.AccountInfo();
        if (info == null)  Console.WriteLine("No User Info Found");
        else    Console.WriteLine(info);
        return;
    }

    protected void UpdateAccount() {
        if (Inputs.YesNoExit("\nWhould you like to update your account information?")) {
            // do {
            //     string input = Inputs.RealInput("What would you like to change?");
            //     bool number = int.TryParse(input, out int choice);

            //     Console.Clear();

            //     if (number) {
            //         switch(choice) {
            //             case 0:
            //                 return 0;
            //             case 1:
            //                 CreateTicket();
            //                 return 1;
            //             case 2:
            //                 ViewTickets();
            //                 return 2;
            //             case 3:
            //                 ViewAccount();
            //                 UpdateAccount();
            //                 return 3;
            //             case 4:
            //                 return 4;
            //             case 5:
            //                 return 5;
            //             default:
            //                 break;
            //         }
            //     }
                    
            //     Console.WriteLine("\n***Unrecognized Input (\"0\" to exit)***\n");
            // } while (true);
        }
        return;
    }
}

public class ManagerMenu : UserMenu
{   
    public ManagerMenu(EmployeeService service) : base(service) {        // COULD make an child class of EmployeeService specifically for manager functionality??!
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
                ChangeRole();
            }
        } while (true);

        return;
    }

    private void ProcessTickets() {
        do {
            Console.Clear();
            Ticket? t = _service.TicketFromQueue();
            if (t == null) {
                Console.Clear();
                Console.WriteLine("There are no unreviewed tickets!\n");
                return;
            }

            Console.WriteLine("Reviewing oldest ticket submission.");
            Console.WriteLine("\nTicket Info: ");

            Console.Write(t.Info());

            _service.ReviewTicket(t, Inputs.YesNoExit("\nApprove ticket?"));

            if (!Inputs.YesNoExit("Would you like to continue with another ticket?"))   return;
        } while (true);
    }

    private void ChangeRole() {
        string user = Inputs.RealInput("Username to change role of: ");
        bool? manager = _service.RoleCheck(user);

        if (manager == null) {
            Console.WriteLine("Username does not exist!");
            return;
        }

        Console.WriteLine($"User Role: {(manager == true ? "Manager" : "Employee")}");

        if (Inputs.YesNoExit("Would you like to change user's role?")) {
            _service.ChangeRole(user);

            Console.WriteLine($"Role of {user} changed!");
        }

        return;
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
                
            Console.WriteLine("\n***Unrecognized Input (\"E\" to exit)***\n");
        } while (true);
    }

    // public static int Number(string msg) {}
}
class Authentication
{
    private List<string> usernames = new List<string>();
    private List<string> passwords = new List<string>();

    public bool Authenticated { get; private set;} = false;

    public void AccessMenu() {
        Console.WriteLine("Welcome to the company's expense reimbursement system");
        bool acceptInput;
        char choice;
        do {
            Console.WriteLine("Do you have a login? (Y/N)");
            string input = Console.ReadLine();
            acceptInput = char.TryParse(input, out choice);

            if (acceptInput) break;
            else {
                Console.WriteLine("Unrecognized Input");
            }
        } while (!acceptInput);

        switch(char.ToLower(choice)) {
            case 'n':
            Register();
            break;
            case 'y':
            LogIn();
            break;
            default:
            Console.WriteLine("ERROR");
            break;
        }

        return;
    }

    private void LogIn() {
        Console.WriteLine("Input Username:");
        string username = Console.ReadLine();
        
        while (!usernames.Contains(username) || String.IsNullOrEmpty(username)) {
            Console.WriteLine("Username Not Recognized");

            Console.WriteLine("Input Username: (\"NEW\" to register)");
            username = Console.ReadLine();

            if (username.ToLower() == "new") {
                Register();
                return;
            }
        }

        int userIndex = usernames.FindIndex(u => u.StartsWith(username));

        Console.WriteLine("Input Password: ");
        string password = Console.ReadLine();
        int attempts = 0;

        while (passwords[userIndex] != password || String.IsNullOrEmpty(password)) {
            if (attempts > 5) {
                Authenticated = false;
                Console.WriteLine("Excess Attempts - Login Failed");
                return;
            }

            Console.WriteLine($"Password Incorrect - {5 - attempts} attempts remain");
            Console.WriteLine("Input Password: (\"E\" to exit)");
            password = Console.ReadLine();

            if (password.ToLower() == "e") {
                Authenticated = false;
                return;
            }

            attempts++;
        }

        Console.WriteLine("Login Successful!");
        Authenticated = true;
        return;
    }

    private void Register() {
        Console.WriteLine("New User Registration");
        Console.WriteLine("New Username: ");
        string username = Console.ReadLine();

        while (!usernames.Contains(username) || String.IsNullOrEmpty(username)) {
            Console.WriteLine("Invalid Input");

            if (usernames.Contains(username)) {
                Console.WriteLine("Username already exists!");
            }

            Console.WriteLine("New Username: (\"E\" to exit)");
            username = Console.ReadLine();

            if (username.ToLower() == "e") {
                Authenticated = false;
                return;
            }
        }

        Console.WriteLine("New Password: (must be at least 6 characters long)");
        string password = Console.ReadLine();

        while (password.Length < 6 || String.IsNullOrEmpty(password)) {
            Console.WriteLine("Invalid Input");

            Console.WriteLine("New Password: (must be at least 6 characters long)");
            password = Console.ReadLine();
        }

        usernames.Add(username);
        passwords.Add(password);

        Console.WriteLine("Login Registered!");
        Authenticated = true;
        return;
    }
}

class Ticket
{
    private int amount;
    private string description;
    private bool reviewed;

    public Ticket()
    {
        inputAmount();
        inputDescription();
        reviewed = false;
    }

    public Ticket(int amount, string description)
    {
        this.amount = amount;
        this.description = description;
        reviewed = false;
    }

    private void inputAmount() 
    {
        return;
    }
    
    private void inputDescription()
    {
        return;
    }
}
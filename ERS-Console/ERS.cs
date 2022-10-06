class Data 
{
    public static List<string> _usernames = new List<string>();
}

class Authentication
{
    private List<string> _usernames = new List<string>();
    private List<string> _passwords = new List<string>();
    private List<Employee> _users = new List<Employee>();
    public bool Authenticated { get; private set;} = false;

    private string _input = "";

    public Authentication() {
        AccessMenu();
    }

    public void AccessMenu() {
        Console.WriteLine("Welcome to the company's expense reimbursement system!");
        do {
            _input = Inputs.realInput("Do you have a login? (y/N)");
            bool letter = char.TryParse(_input, out char choice);

            if (letter) {
                switch(char.ToLower(choice)) {
                    case 'n':
                    Register();
                    return;
                    case 'y':
                    LogIn();
                    return;
                    case 'e':
                    return;
                    default:
                    break;
                }
            }
                
            Console.WriteLine("Unrecognized Input (\"E\" to exit)");
        } while (true);
    }

    private void LogIn() {
        _input = Inputs.realInput("Username:");
        
        while (!_usernames.Contains(_input)) {
            Console.WriteLine("Username Not Recognized");

            _input = Inputs.realInput("Username: (\"new\" to register)");

            if (_input.ToLower() == "new") {
                Register();
                return;
            }
        }

        int userIndex = _usernames.FindIndex(u => u.StartsWith(_input));

        _input = Inputs.realInput("Password: ");
        int attempts = 0;

        while (_passwords[userIndex] != _input) {
            if (attempts > 3 || _input.ToLower() == "e") {
                Authenticated = false;
                Console.WriteLine("Exiting");
                return;
            }

            Console.WriteLine($"Password Incorrect - {3 - attempts} attempts remain");
            _input = Inputs.realInput("Password: (\"E\" to exit)");

            attempts++;
        }

        Console.WriteLine("Login Successful!");
        Authenticated = true;
        return;
    }

    private void Register() {
        do {
            _input = Inputs.realInput("Would you like to register? (y/N)");
            bool letter = char.TryParse(_input, out char choice);

            if (letter) {
                choice = char.ToLower(choice);
                if (choice == 'y') {
                    break;
                }
                else if (choice == 'n' || choice == 'e') {
                    return;
                }
            }
                
            Console.WriteLine("Unrecognized Input (\"E\" to exit)");
        } while (true);

        Console.WriteLine("New User Registration");
        _input = Inputs.realInput("New Username: ");

        while (_usernames.Contains(_input)) {
            Console.WriteLine("Username already exists!");

            _input = Inputs.realInput("New Username: (\"E\" to exit)");

            if (_input.ToLower() == "e") {
                return;
            }
        }

        string username = _input;
        _input = Inputs.realInput("New Password: (must be at least 6 characters long)");

        while (_input.Length < 6) {
            Console.WriteLine("Invalid Input - Too Short");

            _input = Inputs.realInput("New Password: (must be at least 6 characters long)");
        }

        _usernames.Add(username);
        _passwords.Add(_input);
        Data._usernames.Add(username);

        Console.WriteLine("Login Registered!");
        Authenticated = true;

        _users.Add(new Employee());
        return;
    }
}

class Ticket
{
    private decimal _amount;
    private string _description;
    public bool Reviewed { get; private set;} = false;

    public Ticket()
    {
        Amount();
        _description = Inputs.realInput("Expense/ticket description: ");
    }

    public Ticket(int amount, string description)
    {
        _amount = amount;
        _description = description;
    }

    private void Amount() 
    {
        bool isNumber = true;

        do {
            if (!isNumber) 
                Console.WriteLine("Invalid Input");

            string input = Inputs.realInput("How much was the expense? (Numbers Only))");
            isNumber = decimal.TryParse(input, out _amount);
        } while (!isNumber);

        return;
    }

    public string printInfo() {
        return ($"Amount: {_amount}\nDescription: {_description}\n");
    }
}

public static class Inputs
{
    public static string realInput(string msg) {
        do {
            Console.WriteLine(msg);
            string input = Console.ReadLine();

            if (!String.IsNullOrWhiteSpace(input)) {
                return input;
            }
        } while (true);
    }
}
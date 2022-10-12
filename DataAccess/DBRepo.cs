using Models;
using Microsoft.Data.SqlClient;

namespace DataAccess;

public class DBRepo : IERSStorage
{
    private Queue<Ticket> _ticketQueue = new Queue<Ticket>();
    private ConnectionFactory _factory;
    public DBRepo() {
        _factory = new ConnectionFactory();
    }
    
    private SqlDataReader? GetData(string query) {
        try
        {
            using SqlConnection connection = _factory.GetConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) return reader;
        }
        catch (SqlException ex)
        {
            // SeriLog
        }
        return null;
    }
    private void SetData(SqlCommand command) {
        ConnectionFactory factory = new ConnectionFactory();

        try
        {
            using SqlConnection connection = factory.GetConnection();
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            // SeriLog
        }
        return;
    }

    public Employee? GetEmployee(string username) {
        using SqlDataReader? data = GetData($"SELECT * FROM Employees WHERE username = '{username}';");
        if (data == null)   return null;

        return new Employee((string) data["FirstName"], (string) data["LastName"], (string) data["Username"]);
    }
    public Manager? GetManager(string username) {
        using SqlDataReader? data = GetData($"SELECT * FROM Employees WHERE username = '{username}';");
        if (data == null)   return null;

        return new Manager((string) data["FirstName"], (string) data["LastName"], (string) data["Username"]);
    }
    public List<Ticket>? GetTickets(string username) {
        using SqlDataReader? data = GetData($"SELECT * FROM Tickets WHERE Employee = '{username}'");////ERROR CHECK in DATA STUDIO!!!
        if (data == null)   return null;
        
        List<Ticket> tickets = new List<Ticket>();
        while (data.Read()) {
            tickets.Add(new Ticket((decimal) data["Amount"], (string) data["Description"], (string) data["Status"]));
        }
        return tickets;
    }
    public Ticket GetFromQueue() {
        using SqlDataReader data = GetData("SELECT ");////// FINISH Implementation!! Must get from Tickets table what is the top 1 of TicketQueue when sorted descendingly by WaitNum

        return new Ticket((decimal) data["Amount"], (string) data["Description"], (string) data["Status"]);
    }

    public void AddLogIn(string username, string password) {
        using SqlCommand cmd = new SqlCommand("INSERT INTO LogIns (Username, Password) VALUES (@UName, @PWord)");

        cmd.Parameters.AddWithValue("@UName", username);
        cmd.Parameters.AddWithValue("@PWord", password);

        SetData(cmd);
        return;
    }
    public void AddEmployee(string username) {
        using SqlCommand cmd = new SqlCommand($"INSERT INTO Employees (Username) VALUES ('{username}')");

        SetData(cmd);
        return;
    }
    public void NewTicket(Ticket t, string username) {
        using SqlCommand cmd = new SqlCommand("INSERT INTO Tickets (Employee, Amount, Description) VALUES (@UName, @Amount, @Desc)");

        cmd.Parameters.AddWithValue("@UName", username);
        cmd.Parameters.AddWithValue("@Amount", t.Amount);
        cmd.Parameters.AddWithValue("@Desc", t.Description);

        SetData(cmd);
        return;
    }

    public bool CheckUsername(string username) {
        using SqlDataReader? data = GetData($"SELECT * FROM LogIns WHERE Username = '{username}';");
        if (data == null)   return false;
        else    return true;
    }
    public bool VerifyCredentials(string username, string password) {
        using SqlDataReader? data = GetData($"SELECT * FROM LogIns WHERE Username = '{username}';");
        if (((string) data["Password"]) == password)   return true;
        else    return false;
    }
    public bool IsManager(string username) {
        using SqlDataReader? data = GetData($"SELECT * FROM Managers WHERE Username = '{username}';");
        if (data == null)   return false;
        else    return true;
    }

    public int TicketsCount(string username) {
        using SqlDataReader data = GetData($"SELECT COUNT(Number) FROM Tickets WHERE Employee = '{username}'");////ERROR CHECK in DATA STUDIO!!!
        return int.Parse(data.ToString());
    }
    public int QueueLength() {
        using SqlDataReader data = GetData("SELECT COUNT(Number) FROM TicketQueue");///ERROR CHECK in DATA STUDIO!!!
        return int.Parse(data.ToString());
    }
}


public class ConnectionFactory
{
    private const string _connectionString = $"Server=tcp:220926net-sg-ers.database.windows.net,1433;Initial Catalog=ERSDB;Persist Security Info=False;User ID=ers-admin;Password={Secret.password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public SqlConnection GetConnection() {
        return new SqlConnection(_connectionString);
    }
}
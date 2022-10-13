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
    
    private List<string>? GetData(string query, List<string> columns) {
        try
        {
            using SqlConnection connection = _factory.GetConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) {
                List<string> values = new List<string>();

                while (reader.Read()) {
                    foreach (string column in columns) {
                        //if (reader[column].GetType() == typeof(DBNull)) continue;
                        if (reader[column].GetType() == typeof(Int32)) {
                            values.Add(reader[column].ToString());
                            continue;
                        }
                        else if (reader[column].GetType() == typeof(Decimal)) {
                            values.Add(reader[column].ToString());
                            continue;
                        }
                        else if (reader[column].GetType() == typeof(DBNull)) continue;

                        values.Add((string) reader[column]);
                    }
                }

                return values;
            }

            connection.Close();
        }
        catch (SqlException ex)
        {
            // SeriLog
        }
        return null;
    }
    private void SetData(string command, List<string> parameters, List<string> values) {
        try
        {
            using SqlConnection connection = _factory.GetConnection();
            connection.Open();

            using SqlCommand cmd = new SqlCommand(command, connection);

            for (int i = 0; i < parameters.Count; i++) {
                cmd.Parameters.AddWithValue(parameters[i], values[i]);
            }

            cmd.ExecuteNonQuery();

            connection.Close();
        }
        catch (SqlException ex)
        {
            // SeriLog
        }
        return;
    }

    public Employee? GetEmployee(string username) {
        List<string>? data = GetData($"SELECT * FROM Employees WHERE username = '{username}';", new List<string> {"FirstName", "MiddleName", "LastName", "Username"});
        if (data == null)   return null;

        if (data.Count < 4) return new Employee(data[0], data[1], data[2]);
        else    return new Employee(data[0], data[1], data[2], data[3]);
    }
    public Manager? GetManager(string username) {
        List<string>? data = GetData($"SELECT * FROM Employees WHERE username = '{username}';", new List<string> {"FirstName", "MiddleName", "LastName", "Username"});
        if (data == null)   return null;

        if (data.Count < 4) return new Manager(data[0], data[1], data[2]);
        else    return new Manager(data[0], data[1], data[2], data[3]);
    }
    public List<Ticket>? GetTickets(string username) {
        List<string>? data = GetData($"SELECT * FROM Tickets WHERE Employee = '{username}';", new List<string> {"Amount", "Description", "Status"});////ERROR CHECK in DATA STUDIO!!!
        if (data == null)   return null;
        
        List<Ticket> tickets = new List<Ticket>();
        for (int i = 0; i < data.Count; i += 3) {
            tickets.Add(new Ticket(decimal.Parse(data[i]), data[i+1], data[i+2]));
        }

        return tickets;
    }
    public Ticket GetFromQueue() {
        List<string>? data = GetData("SELECT ", new List<string> {"Amount", "Description", "Status"});////// FINISH Implementation!! Must get from Tickets table what is the top 1 of TicketQueue when sorted descendingly by WaitNum

        return new Ticket(decimal.Parse(data[0]), data[1], data[2]);
    }

    public void AddLogIn(string username, string password) {
        SetData("INSERT INTO LogIns (Username, Password) VALUES (@UName, @PWord)", new List<string> {"@UName", "@PWord"}, new List<string> {username, password});
        return;
    }
    public void AddEmployee(string username) {
        SetData($"INSERT INTO Employees (Username) VALUES (@UName)", new List<string> {"@UName"}, new List<string> {username});
        return;
    }
    public void NewTicket(Ticket t, string username) {
        SetData("INSERT INTO Tickets (Employee, Amount, Description) VALUES (@UName, @Amount, @Desc)", new List<string> {"@UName", "@Amount", "@Desc"}, new List<string> {username, t.Amount.ToString(), t.Description});
        return;
    }

    public bool CheckUsername(string username) {
        List<string>? data = GetData($"SELECT * FROM LogIns WHERE Username = '{username}';", new List<string> {"Username"});
        if (data == null)   return false;
        else    return true;
    }
    public bool VerifyCredentials(string username, string password) {
        List<string>? data = GetData($"SELECT * FROM LogIns WHERE Username = '{username}';", new List<string> {"Password"});
        if (data[0] == password)   return true;
        else    return false;
    }
    public bool IsManager(string username) {
        List<string>? data = GetData($"SELECT * FROM Managers WHERE Username = '{username}';", new List<string> {"Username"});
        if (data == null)   return false;
        else    return true;
    }

    public int TicketsCount(string username) {
        List<string>? data = GetData($"SELECT COUNT(Number) AS NumTickets FROM Tickets WHERE Employee = '{username}'", new List<string> {"NumTickets"});////ERROR CHECK in DATA STUDIO!!!
        return int.Parse(data[0]);
    }
    public int QueueLength() {
        List<string>? data = GetData("SELECT COUNT(Number) AS NumTickets FROM TicketQueue", new List<string> {"NumTickets"});///ERROR CHECK in DATA STUDIO!!!
        return int.Parse(data[0]);
    }
}


public class ConnectionFactory
{
    private const string _connectionString = $"Server=tcp:220926net-sg-ers.database.windows.net,1433;Initial Catalog=ERSDB;Persist Security Info=False;User ID=ers-admin;Password={Secret.password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public SqlConnection GetConnection() {
        return new SqlConnection(_connectionString);
    }
}
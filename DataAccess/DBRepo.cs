using Models;
using Microsoft.Data.SqlClient;

namespace DataAccess;

public class DBRepo : IERSStorage
{
    private ConnectionFactory _factory;
    public DBRepo() {
        _factory = new ConnectionFactory();
    }
    
    private string? GetValue(string query, string column) {
        string? value = null;
        try
        {
            using SqlConnection connection = _factory.GetConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) {
                if (reader.Read()) {
                    if (reader[column].GetType() == typeof(Int32))
                        value = reader[column].ToString();
                    else    
                        value = (string) reader[column];
                }
            }

            connection.Close();
        }
        catch (SqlException ex)
        {
            // SeriLog
        }
        return value;
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
                        if (reader[column].GetType() == typeof(DBNull)) continue;
                        else if (reader[column].GetType() == typeof(Int32)) {
                            values.Add(reader[column].ToString());
                            continue;
                        }
                        else if (reader[column].GetType() == typeof(Decimal)) {
                            values.Add(reader[column].ToString());
                            continue;
                        }

                        values.Add((string) reader[column]);
                    }
                }
                connection.Close();
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

    public void AddLogIn(string username, string password) {
        SetData("INSERT INTO LogIns (Username, Password) VALUES (@UName, @PWord)", new List<string> {"@UName", "@PWord"}, new List<string> {username, password});
        return;
    }
    public void AddEmployee(string username) {
        SetData($"INSERT INTO Employees (Username) VALUES (@UName)", new List<string> {"@UName"}, new List<string> {username});
        return;
    }
    public int NewTicket(Ticket t) {
        SetData("INSERT INTO Tickets (Employee, Amount, Description) VALUES (@UID, @Amount, @Desc)", new List<string> {"@UID", "@Amount", "@Desc"}, new List<string> {t.User.ToString(), t.Amount.ToString(), t.Description});
        return int.Parse(GetValue($"SELECT TOP 1 * FROM Tickets WHERE Employee = {t.User} ORDER BY Number DESC;", "Number")!);
    }

    public List<Ticket>? GetTickets(int? userId) {
        string q;
        bool s = false;
        if (userId == 0) {
            //data = GetData("SELECT * FROM Tickets WHERE Reviewer is null ORDER BY Number DESC", new List<string> {"Number", "Employee", "Amount", "Description", "Status"});
            q = "SELECT * FROM Tickets WHERE Reviewer is null ORDER BY Number DESC";
            s = false;
        }
        else {
            q = $"SELECT * FROM Tickets WHERE Employee = {userId};";
            s = true;
        }

        List<string>? data = GetData(q, new List<string> {"Number", "Employee", "Amount", "Description", "Status", "Reviewer"});
        if (data == null)   return null;
        
        List<Ticket> tickets = new List<Ticket>();
        for (int i = 0; i < data.Count; i += 5) {
            if (s && data.Count % 6 == 0) {   
                    tickets.Add(new Ticket(int.Parse(data[i]), int.Parse(data[i+1]), decimal.Parse(data[i+2]), data[i+3], data[i+4], int.Parse(data[i+5])));
                    i++;}
                else {
                tickets.Add(new Ticket(int.Parse(data[i]), int.Parse(data[i+1]), decimal.Parse(data[i+2]), data[i+3], data[i+4], 0));//(data[i+5] == null ? 0 : int.Parse(data[i+5]))));
                }
                //
            //else
                //tickets.Add(new Ticket(int.Parse(data[i]), int.Parse(data[i+1]), decimal.Parse(data[i+2]), data[i+3], data[i+4], 0));//(data[i+5] == null ? 0 : int.Parse(data[i+4]))));
            //tickets.Add(new Ticket(1, 1, 10, data[i+6], data[i+6], 0));
        }

        return tickets;
    }

    public void UpdateStatus(int ticketNum, int reviewerId, string decision) {
        SetData("UPDATE Tickets SET Status = @Decision, Reviewer = @Manager WHERE Number = @TicketID", new List<string> {"@Decision", "@Manager", "@TicketID"}, new List<string> {decision, reviewerId.ToString(), ticketNum.ToString()});
        return;
    }

    public bool UsernameExists(string username) {
        string? user = GetValue($"SELECT * FROM LogIns WHERE Username = '{username}';", "Username");
        if (user == null)   return false;
        else    return true;
    }
    public bool VerifyCredentials(string username, string password) {
        string? pass = GetValue($"SELECT * FROM LogIns WHERE Username = '{username}';", "Password");
        if (pass == password)   return true;
        else    return false;
    }
    public bool IsManager(int id) {
        string? data = GetValue($"SELECT * FROM Managers WHERE EmpId = {id};", "EmpId");
        if (data == null)   return false;
        else    return true;
    }

    public int UserId(string username) {
        string? id = GetValue($"SELECT * FROM Employees WHERE Username = '{username}';", "Id");
        if (id == null) return 0;
        else    return int.Parse(id);
    }

    



















    // public Employee? GetEmployee(string username) {
    //     List<string>? data = GetData($"SELECT * FROM Employees WHERE username = '{username}';", new List<string> {"FirstName", "MiddleName", "LastName", "Username"});
    //     if (data == null)   return null;

    //     if (data.Count < 4) return new Employee(data[0], data[1], data[2]);
    //     else    return new Employee(data[0], data[1], data[2], data[3]);
    // }
    // public Manager? GetManager(string username) {
    //     List<string>? data = GetData($"SELECT * FROM Employees WHERE username = '{username}';", new List<string> {"FirstName", "MiddleName", "LastName", "Username"});
    //     if (data == null)   return null;

    //     if (data.Count < 4) return new Manager(data[0], data[1], data[2]);
    //     else    return new Manager(data[0], data[1], data[2], data[3]);
    // }
    // public Ticket? GetFromQueue() {          /// SHOULD it also get the employee who submitted it?
    //     List<string>? data = GetData("SELECT TOP 1 * FROM Tickets WHERE Reviewer is null ORDER BY Number DESC", new List<string> {"Employee", "Amount", "Description", "Status"});////// FINISH Implementation!! Must get from Tickets table what is the top 1 of TicketQueue when sorted descendingly by WaitNum
    //     if (data == null)   return null;
    //     else    return new Ticket(data[0], decimal.Parse(data[1]), data[2], data[3]);
    // }

    // public bool CheckUsername(string username) {
    //     List<string>? data = GetData($"SELECT * FROM LogIns WHERE Username = '{username}';", new List<string> {"Username"});
    //     if (data == null)   return false;
    //     else    return true;
    // }

    // public int TicketsCount(string username) {
    //     List<string>? data = GetData($"SELECT COUNT(Number) AS NumTickets FROM Tickets WHERE Employee = '{username}'", new List<string> {"NumTickets"});////ERROR CHECK in DATA STUDIO!!!
    //     return int.Parse(data[0]);
    // }
    // public int QueueLength() {
    //     List<string>? data = GetData("SELECT COUNT(Number) AS NumTickets FROM TicketQueue", new List<string> {"NumTickets"});///ERROR CHECK in DATA STUDIO!!!
    //     return int.Parse(data[0]);
    // }
}


public class ConnectionFactory
{
    private const string _connectionString = $"Server=tcp:220926net-sg-ers.database.windows.net,1433;Initial Catalog=ERSDB;Persist Security Info=False;User ID=ers-admin;Password={Secret.password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public SqlConnection GetConnection() {
        return new SqlConnection(_connectionString);
    }
}
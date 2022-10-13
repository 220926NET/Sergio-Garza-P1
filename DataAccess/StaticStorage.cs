/*

using Models;
using System.Text.Json;


namespace DataAccess;

public static class StaticStorage
{
    private static Dictionary<string, string> _logIns = new Dictionary<string, string> {
        {"alejandro.vasquez", "123456"}, {"alexander.soudry", "123456"}, {"bryan.gewickey", "123456"}, {"chi.ntum", "123456"}, {"danielle.flock", "123456"}, 
        {"denis.espino", "123456"}, {"duncan.caruthers", "123456"}, {"emmanuel.morales", "123456"}, {"gregory.sabado", "123456"}, {"haizhen.huang", "123456"}, 
        {"ilgaz.arslan", "123456"}, {"jacob.allen", "123456"}, {"james.monty", "123456"}, {"kevin.dean", "123456"}, {"manyo.nkiri", "123456"}, 
        {"xavier.hall", "123456"}, {"paul.smithey", "123456"}, {"rushay.hays", "123456"}, {"sergio.garza", "123456"}, {"shelique.richardson", "123456"}, 
        {"tarikuwa.demeke", "123456"}, {"juniper.song", "Password-123456"}
    };
    private static Dictionary<string, Employee> _employees = new Dictionary<string, Employee> 
    {
        {"alejandro.vasquez", new Employee("Alejandro", "Vasquez")}, {"alexander.soudry", new Employee("Alexander", "Soudry")}, 
        {"bryan.gewickey", new Employee("Bryan", "Gewickey")}, {"chi.ntum", new Employee("Chi", "Ntum")}, {"danielle.flock", new Employee("Danielle", "Flock")}, 
        {"denis.espino", new Employee("Denis", "Espino")}, {"duncan.caruthers", new Employee("Duncan", "Caruthers")}, 
        {"emmanuel.morales", new Employee("Emmanuel", "Morales")}, {"gregory.sabado", new Employee("Gregory", "Sabado")}, 
        {"haizhen.huang", new Employee("Haizhen", "Huang")}, {"ilgaz.arslan", new Employee("Ilgaz", "Arslan")}, {"jacob.allen", new Employee("Jacob", "Allen")}, 
        {"james.monty", new Employee("James", "Monty-Carbonari")}, {"kevin.dean", new Employee("Kevin", "Dean")}, {"manyo.nkiri", new Employee("Manyo", "Ojong", "Nkiri")}, 
        {"xavier.hall", new Employee("Merced Xavier", "Perez Hall")}, {"paul.smithey", new Employee("Paul", "Sheffield", "Smithey")}, 
        {"rushay.hays", new Employee("Rushay", "Andrea", "Hays")}, {"sergio.garza", new Employee("Sergio", "Ezequiel", "Garza")}, 
        {"shelique.richardson", new Employee("Shelique", "Richardson")}, {"tarikuwa.demeke", new Employee("Tarikuwa", "Demeke")}
    };
    private static Dictionary<string, Manager> _managers = new Dictionary<string, Manager> {
        {"juniper.song", new Manager("Juniper", "Song")}
    };
    private static Queue<Ticket> _ticketQueue = new Queue<Ticket>();

    public static void GetFilesInfo() {
        if (File.Exists(@".\logins.json")) {
            _logIns = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(@".\logins.json"))!;
        }
        if (File.Exists(@".\employees.json")) {
            _employees = JsonSerializer.Deserialize<Dictionary<string, Employee>>(File.ReadAllText(@".\employees.json"))!;
        }
        if (File.Exists(@".\managers.json")) {
            _managers = JsonSerializer.Deserialize<Dictionary<string, Manager>>(File.ReadAllText(@".\managers.json"))!;
        }
        return;
    }
    public static void UpdateFiles() {
        File.WriteAllText(@".\logins.json", JsonSerializer.Serialize(_logIns));
        File.WriteAllText(@".\employees.json", JsonSerializer.Serialize(_employees));
        File.WriteAllText(@".\managers.json", JsonSerializer.Serialize(_managers));
        File.WriteAllText(@".\queue.json", JsonSerializer.Serialize(_ticketQueue));
        return;
    }

    public static Employee GetEmployee(string username) {
        return _employees[username];
    }
    public static Manager GetManager(string username) {
        return _managers[username];
    }
    public static Ticket GetFromQueue() {
        return _ticketQueue.Dequeue();
    }

    public static void AddLogIn(string username, string password) {
        _logIns.Add(username, password);
        return;
    }
    public static void AddEmployee(string username) {
        _employees.Add(username, new Employee());
        return;
    }
    public static void NewTicket(Ticket t) {
        _ticketQueue.Enqueue(t);
        return;
    }
    public static void EmployeeToManager(string username) {
        _managers.Add(username, (Manager)_employees[username]);
        _employees.Remove(username);
        return;
    }

    public static bool CheckUsername(string username) {
        if (_logIns.ContainsKey(username.ToLower()))    return true;
        else    return false;
    }
    public static bool VerifyCredentials(string username, string password) {
        if (_logIns[username] == password)  return true;
        else    return false;
    }
    public static bool IsManager(string username) {
        if (_managers.ContainsKey(username.ToLower()))  return true;
        else    return false;
    }
    public static int QueueLength() {
        return _ticketQueue.Count;
    }
}

*/
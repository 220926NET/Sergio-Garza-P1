using Models;
using DataAccess;

namespace Services;

public class Authentication
{
    private bool _authenticated;
    public Authentication() {
        _authenticated = false;
        //StaticStorage.GetInfo();
    }

    public bool UsernameExists(string username) {
        if (StaticStorage.CheckUsername(username)) {
            return true;
        }
        else    return false;
    }

    public bool LogIn(string username, string password) {
        if (UsernameExists(username)) {
            if (StaticStorage.VerifyCredentials(username, password)) {
                _authenticated = true;
                return true;
            }
        }
        _authenticated = false;
        return false;
    }

    public bool Register(string username, string password) {
        if (!UsernameExists(username)) {
            StaticStorage.AddLogIn(username, password);
            return true;
        }
        _authenticated = false;
        return false;
    }
}

public class ERSService
{
    public static bool ManagerCheck(string username) {
        if (StaticStorage.IsManager(username))  return true;
        else    return false;
    }
}

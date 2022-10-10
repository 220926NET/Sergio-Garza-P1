//using Models;
using Services;

AuthenticationMenu access = new AuthenticationMenu();
access.AccessMenu();

if (access.LoggedIn == false) {
    Console.WriteLine("Access Rejected");
    Environment.Exit(0);
}

Console.WriteLine("Login Successful!");

if (ERSService.ManagerCheck()) {
    ManagerMenu mMenu = new ManagerMenu();
}
else
    UserMenu.Repeat();


access.End();
Console.WriteLine("Exiting System");












access.AccessMenu();

if (access.LoggedIn == false) {
    Console.WriteLine("Access Rejected");
    Environment.Exit(0);
}

Console.WriteLine("Login Successful!");

if (ERSService.ManagerCheck()) {
    ManagerMenu mMenu = new ManagerMenu();
}
else
    UserMenu.Repeat();


access.End();
Console.WriteLine("Exiting System");
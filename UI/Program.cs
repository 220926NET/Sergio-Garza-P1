using Models;
using Services;

AuthenticationMenu access = new AuthenticationMenu();
access.AccessMenu();

if (access.LoggedIn == false) {
    Console.WriteLine("Access Rejected");
    Environment.Exit(0);
}

Console.WriteLine("Login Successful!");

if (ERSService.ManagerCheck(access.Username)) {
    ManagerMenu mMenu = new ManagerMenu();
}
else
    MainMenu.Repeat();
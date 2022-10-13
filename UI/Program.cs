using Menus;

AuthenticationMenu access = new AuthenticationMenu();
access.AccessMenu();

if (access.Service == null) {
    Console.WriteLine("Access Rejected");
    Environment.Exit(0);
}

Console.WriteLine("Login Successful!");

if (access.Service.ManagerCheck()) {
    ManagerMenu mMenu = new ManagerMenu(access.Service);
}
else {
    UserMenu eMenu = new UserMenu(access.Service);
    eMenu.Repeat();
}


access.End();
Console.WriteLine("Exiting System");











/*
access.AccessMenu();

if (access.Service == null) {
    Console.WriteLine("Access Rejected");
    Environment.Exit(0);
}

Console.WriteLine("Login Successful!");

if (access.Service.ManagerCheck()) {
    ManagerMenu mMenu = new ManagerMenu(access.Service);
}
else {
    UserMenu uMenu = new UserMenu(access.Service);
    uMenu.Repeat();
}

access.End();
Console.WriteLine("Exiting System");
*/
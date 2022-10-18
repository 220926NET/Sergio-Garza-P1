using Menus;

AuthenticationMenu access = new AuthenticationMenu();
access.AccessMenu();

if (access.Service == null) {
    Console.WriteLine("\n****************\nAccess Rejected\n****************\n");
    Environment.Exit(0);
}

Console.Clear();
Console.WriteLine("-----------------\nLogin Successful!\n-----------------\n");

if (access.Service.ManagerCheck()) {
    ManagerMenu mMenu = new ManagerMenu(access.Service);
}
else {
    UserMenu eMenu = new UserMenu(access.Service);
    eMenu.Repeat();
}


access.End();
Console.WriteLine("*************\nSystem Exited\n*************\n");











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
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Authentication access = new Authentication();

if (access.Authenticated) {
    Console.WriteLine("Access Granted");
}
else {
    Console.WriteLine("Access Failure");
}
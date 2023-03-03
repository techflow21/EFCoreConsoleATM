
using EFCoreATM_Domain;

namespace EFCoreATM_Interface
{
    public class Menu
    {
        AdminService adminService = new();
        CustomerService customerService = new();
        AdminMenu adminMenu = new();
        CustomerMenu customerMenu = new();
        public void Options()
        {
            bool isRunning = true;

            while (isRunning)
            {
                adminService.DefaultAdmins();

                Console.Write("\n\t Select your Menu choice: \n\t ======================= \n\t 1. Login as an Admin\n\t 2. Login as a Customer\n\t 3. Exit\n\n\t ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("\n\t Enter Username:\n\t ");
                        string username = Console.ReadLine();

                        Console.Write("\n\t Enter Password:\n\t ");
                        string password = Console.ReadLine();

                        var check = adminService.Login(username, password);

                        if (check)
                        {
                            Console.WriteLine($"\n\t Admin Logged in Successful!");
                            adminMenu.Option();
                        }
                        Console.WriteLine("\n\t Invalid Username or Password");
                        break;

                    case "2":
                        Console.Clear();

                        Console.Write("\n\t Enter your Account Number:\n\t ");
                        string accountNumber = Console.ReadLine();

                        Console.Write("\n\t Enter your ATM Pin:\n\t ");
                        string atmPin = Console.ReadLine();

                        check = customerService.Login(accountNumber, atmPin);

                        if (check)
                        {
                            Console.WriteLine($"\n\t Customer Logged in Successful!");
                            customerMenu.Option();
                        }
                        Console.WriteLine("\n\t Invalid Username or Password");
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("\n\t Thank you for using our ATM Service");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("\n\t You entered wrong choice, try again");
                        break;

                }

                Utility.ContinueOperation();
            }
        }
    }
}

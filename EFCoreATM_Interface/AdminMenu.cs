using EFCoreATM_Data.Models;
using EFCoreATM_Domain;

namespace EFCoreATM_Interface
{
    public class AdminMenu
    {
        AdminService adminService = new();
        public void Option()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Write("\n\t Select your choice \n\t ==================== \n\t 1. To Load ATM with Cash\n\t 2. To View ATM Balance\n\t 3. To Register New Customer\n\t 4. To View All Registered Customers \n\t 5. To Edit Customer's Details \n\t 6. To Deposit Cash to Customer's Account \n\t 7. To Deactivate a Customer\n\t 8. To Re-activate a Customer\n\t 9. To View all ATM Transactions \n\t 10.To Logout\n\n\t ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("\n\t Enter Amount to Load: \n\t ");

                        string amount = Console.ReadLine();
                        adminService.LoadAtm(decimal.Parse(amount));
                        break;

                    case "2":
                        Console.Clear();
                        adminService.CheckAtmBalance();
                        break;

                    case "3":
                        Console.Clear();

                        Console.Write("\n\t Enter First Name: \n\t ");
                        string firstName = Console.ReadLine();

                        Console.Write("\n\t Enter Last Name:\n\t ");
                        string lastName = Console.ReadLine();

                        Console.Write("\n\t Enter Email Address:\n\t ");
                        string email = Console.ReadLine();

                        Console.Write("\n\t Enter Address:\n\t ");
                        string address = Console.ReadLine();

                        Console.Write("\n\t Enter Phone Number:\n\t ");
                        string phone = Console.ReadLine();

                        Console.Write("\n\t Enter Account Type (e.g. \"Savings\", \"Current\", \"Investment\"):\n\t ");
                        string accountType = Console.ReadLine();

                        string accountNumber = "30000" + new Random().Next(10000, 99999).ToString();

                        string defaultPin = new Random().Next(1000, 9999).ToString();

                        var customer = new Customer
                        {
                            FirstName = firstName, LastName = lastName, Email = email,
                            Address = address, PhoneNumber = phone, AccountNumber = accountNumber,
                            AccountType = accountType, AccountBalance = 0, DefaultAtmPin = defaultPin,
                            NewAtmPin = "", IsActive = false, RegisteredDate = DateTime.Now,
                        };

                        adminService.RegisterCustomer(customer);
                        break;

                    case "4":
                        Console.Clear();
                        adminService.ViewAllCustomers();
                        break;

                    case "5":
                        Console.Clear();
                        Console.Write("\n\t Enter customer account number to edit: \n\t ");
                        accountNumber = Console.ReadLine();

                        adminService.EditCustomer(accountNumber);
                        break;

                    case "6":
                        Console.Clear();
                        Console.Write("\n\t Enter customer's account number: \n\t ");
                        accountNumber = Console.ReadLine();

                        Console.Write("\n\t Enter amount to deposit: \n\t ");
                        amount = Console.ReadLine();

                        adminService.Deposit(accountNumber, decimal.Parse(amount));
                        break;

                    case "7":
                        Console.Clear();

                        Console.Write("\n\t Enter customer's account number to deactivate: \n\t ");
                        accountNumber = Console.ReadLine();

                        adminService.DeActivateCustomer(accountNumber);
                        break;

                    case "8":
                        Console.Clear();

                        Console.Write("\n\t Enter customer's account number to re-activate: \n\t ");
                        accountNumber = Console.ReadLine();

                        adminService.ReActivateCustomer(accountNumber);
                        break;

                    case "9":
                        Console.Clear();
                        adminService.ViewAllTransactions();
                        break;

                    case "10":
                        Console.Clear();

                        Menu menu = new();
                        menu.Options();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\n\t You entered invalid choice, try again");
                        break;
                }

                isRunning = Utility.ContinueOperation();
            }
        }
    }
}

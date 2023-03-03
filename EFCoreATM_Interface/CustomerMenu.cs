
using EFCoreATM_Domain;

namespace EFCoreATM_Interface
{
    public class CustomerMenu
    {
        CustomerService customerService = new();
        public void Option()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Write("\n\t Select your choice \n\t ===================== \n\t 1. View Account Details\n\t 2. Check Account Balance\n\t 3. Transfer Money\n\t 4. Withdraw Money\n\t 5. View Account Transactions\n\t 6. Change Default ATM PIN \n\t 7. Logout\n");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        customerService.ViewAccountDetails();
                        break;
                    case "2":
                        customerService.CheckAccountBalance();
                        break;
                    case "3":
                        Console.Clear();

                        Console.Write("\n\t Enter receiver's account number: \n\t ");
                        string accountNumber = Console.ReadLine();

                        Console.Write("\n\t Enter amount to transfer: \n\t ");
                        string amount = Console.ReadLine();

                        customerService.Transfer(accountNumber, decimal.Parse(amount));
                        break;
                    case "4":
                        Console.Clear();

                        Console.Write("\n\t Enter amount to withdraw: \n\t ");
                        amount = Console.ReadLine();

                        customerService.Withdraw(decimal.Parse(amount));
                        break;
                    case "5":
                        Console.Clear();
                        customerService.ViewAccountTransactions();
                        break;

                    case "6":
                        Console.Clear();
                        Console.Write("\n\t Enter current ATM pin: \n\t ");
                        string currentPin = Console.ReadLine();

                        customerService.ChangeAtmPin(currentPin);
                        break;

                    case "7":
                        Console.Clear();
                        Menu menu = new();

                        menu.Options();
                        break;

                    default:
                        Console.WriteLine("\n\t Invalid choice, please try again.");
                        break;
                }

                isRunning = Utility.ContinueOperation();
            }
        }
    }
}

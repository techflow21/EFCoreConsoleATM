using EFCoreATM_Data;
using EFCoreATM_Data.Models;

namespace EFCoreATM_Domain;

public class AdminService
{
    AtmDbContextFactory atmDbContextFactory = new AtmDbContextFactory();
    private static Admin _currentAdmin;

    public void DefaultAdmins()
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        if (context.Admins.Any())
        {
            return;
        }

        var admin1 = new Admin
        {
            FirstName = "John",
            LastName = "Sussan",
            UserName = "admin",
            Password = "1111"
        };

        var admin2 = new Admin
        {
            FirstName = "Success",
            LastName = "David",
            UserName = "admin2",
            Password = "2222"
        };

        context.Admins.AddRange(admin1, admin2);
        context.SaveChanges();
    }


    public bool Login(string username, string password)
    {
        AtmDbContext context = atmDbContextFactory.CreateDbContext(null);

        var admin = context.Admins.FirstOrDefault(a => a.UserName == username && a.Password == password);
        if (admin != null)
        {
            _currentAdmin = admin;
            return true;
        }
        else
        {
            return false;
        }
    }


    public void LoadAtm(decimal amount)
    {
        using (var context = atmDbContextFactory.CreateDbContext(null))
        {
            try
            {
                var atmMachine = new AtmMachine
                {
                    AtmBalance = amount,
                    LoadDate = DateTime.Now
                };

                context.AtmMachine.Add(atmMachine);
                context.SaveChanges();

                Console.WriteLine($"\n\t ATM loaded with ${amount} successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\t Error occurs while loading ATM with Cash try again later.");
            }
        }
    }


    public void CheckAtmBalance()
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var atm = context.AtmMachine.FirstOrDefault();

        if (atm != null)
        {
            Console.WriteLine($"\n\t ATM Balance: ${atm.AtmBalance}");
        }
        else
        {
            Console.WriteLine("\n\t Error occurs while processing your request, Try again later.");
        }
    }


    public void RegisterCustomer(Customer customer)
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        context.Customers.Add(customer);
        context.SaveChanges();

        Console.WriteLine("\n\t New customer registration was succesful!");
    }


    public void EditCustomer(string accountNumber)
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var customer = context.Customers.FirstOrDefault(c => c.AccountNumber == accountNumber);

        if (customer == null)
        {
            Console.WriteLine($"\n\t Customer with account number {accountNumber} does not exist.");
            return;
        }

        Console.Write("\n\t Enter new first name (leave empty to skip): \n\t ");
        var firstName = Console.ReadLine();

        if (!string.IsNullOrEmpty(firstName))
        {
            customer.FirstName = firstName;
        }

        Console.Write("\n\t Enter new last name (leave empty to skip): \n\t ");
        var lastName = Console.ReadLine();

        if (!string.IsNullOrEmpty(lastName))
        {
            customer.LastName = lastName;
        }

        Console.Write("\n\t Enter new email address (leave empty to skip): \n\t ");
        var email = Console.ReadLine();

        if (!string.IsNullOrEmpty(email))
        {
            customer.Email = email;
        }

        Console.Write("\n\t Enter new phone number (leave empty to skip): \n\t ");
        var phone = Console.ReadLine();

        if (!string.IsNullOrEmpty(phone))
        {
            customer.PhoneNumber = phone;
        }

        try
        {
            context.Customers.Update(customer);
            context.SaveChanges();
            Console.WriteLine("\n\t Customer details updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\t An error occurred while updating customer details: {ex.Message}");
        }
    }


    public void Deposit(string accountNumber, decimal amount)
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var customer = context.Customers.FirstOrDefault(c => c.AccountNumber == accountNumber);

        if (customer == null)
        {
            Console.WriteLine("\n\t Customer not found, try again ");
            return;
        }

        customer.AccountBalance += amount;

        var transaction = new TransactionDetail
        {
            TransactionType = "Deposit",
            Sender = "Admin",
            Receiver = customer.AccountNumber,
            TransactionDate = DateTime.Now,
            TransactedAmount = amount
        };

        context.Transactions.Add(transaction);
        context.SaveChanges();

        Console.WriteLine($"\n\t {amount:C} deposited successfully to {customer.FirstName} {customer.LastName}'s account.");
    }


    public void ViewAllCustomers()
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var customers = context.Customers.ToList();

        Console.WriteLine($"\n List of all registered customers:\n ================================= \n {"Id",-10} {"FirstName",-15} {"LastName",-15} {"AccountNumber",-15} {"AccountBalance",-15} {"DefaultPin",-10} {"NewPin",-10} {"AccountType",-15} {"Date_Registered",-20}");
        foreach (var customer in customers)
        {
            Console.WriteLine($"\n {customer.Id,-10} {customer.FirstName,-15} {customer.LastName,-15} {customer.AccountNumber,-15} {customer.AccountBalance,-15} {customer.DefaultAtmPin,-10} {customer.NewAtmPin,-10} {customer.AccountType,-15} {customer.RegisteredDate,-20}\n");
        }
    }


    public void ViewAllTransactions()
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var transactions = context.Transactions
            .Where(t => t.TransactionType != "ATM Load")
            .ToList();

        Console.WriteLine($"\n\t List of all transactions:\n\t ============================\n\t {"Trans_Type",-15} {"Amount",-20} {"Sender",-20} {"Receiver",-20} {"Date/Time",-20}");
        foreach (var transaction in transactions)
        {
            Console.WriteLine($"\n\t {transaction.TransactionType,-15} ${transaction.TransactedAmount,-20} {transaction.Sender,-20} {transaction.Receiver,-20} {transaction.TransactionDate,-20}");
        }
    }


    public void DeActivateCustomer(string accountNumber)
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var customer = context.Customers.FirstOrDefault(c => c.AccountNumber == accountNumber);
        if (customer == null)
        {
            Console.WriteLine("\n\t Customer not found.");
            return;
        }

        customer.IsActive = false;
        context.SaveChanges();

        Console.WriteLine($"\n\t {customer.FirstName} {customer.LastName}'s account has been deactivated!");
    }


    public void ReActivateCustomer(string accountNumber)
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var customer = context.Customers.FirstOrDefault(c => c.AccountNumber == accountNumber);
        if (customer == null)
        {
            Console.WriteLine("\n\t Customer not found.");
            return;
        }
        customer.IsActive = true;
        context.SaveChanges();

        Console.WriteLine($"\n\t {customer.FirstName} {customer.LastName}'s account has been re-activated!");
    }
}

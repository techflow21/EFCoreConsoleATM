using EFCoreATM_Data;
using EFCoreATM_Data.Models;

namespace EFCoreATM_Domain;

public class CustomerService
{
    AtmDbContextFactory atmDbContextFactory = new AtmDbContextFactory();

    private static decimal _atmTotalBalance;
    private static Customer _currentCustomer;

    public bool Login(string accountNumber, string atmPin)
    {
        AtmDbContext context = atmDbContextFactory.CreateDbContext(null);

        var inactiveCustomer = context.Customers.FirstOrDefault(c => c.DefaultAtmPin == atmPin && c.NewAtmPin == null);

        var activeCustomer = context.Customers.FirstOrDefault(c => c.AccountNumber == accountNumber && c.DefaultAtmPin == atmPin && c.NewAtmPin != null);

        if (inactiveCustomer != null)
        {
            Console.Write("\n\t You account is presently inactive, change your default ATM Pin to activate.\n\t Enter your Default ATM Pin: \n\t ");
            var oldAtmPin = Console.ReadLine();
            
            ChangeAtmPin(oldAtmPin);
        }

        if (activeCustomer != null)
        {
            _currentCustomer = activeCustomer;
            return true;
        }
        else
        {
            return false;
        }
    }


    public void ViewAccountDetails()
    {
        Console.WriteLine($"\n\t Customer Details: \n\t ===================\n\t Full Name: {_currentCustomer.FirstName} {_currentCustomer.LastName} \t Phone Number: {_currentCustomer.PhoneNumber}\n\t Account Number: {_currentCustomer.AccountNumber} \t Account Type: {_currentCustomer.AccountType} \n\t Account Balance: {_currentCustomer.AccountBalance:C} \t Registered Date: {_currentCustomer.RegisteredDate}");
    }


    public void CheckAccountBalance()
    {
        Console.WriteLine($"\n\t Your Account Balance: {_currentCustomer.AccountBalance:C}");
    }

    public void Transfer(string accountNumber, decimal transferAmount)
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var _atmTotalBalance = context.AtmMachine.Sum(a => a.AtmBalance);

        var receiver = context.Customers.FirstOrDefault(c => c.AccountNumber == accountNumber);

        if (receiver == null)
        {
            Console.WriteLine("\n\t Receiver's account not found.");
            return;
        }

        if (receiver.AccountNumber == accountNumber)
        {
            Console.WriteLine("\n\t Self transfer is not allow, try other operations.");
            return;
        }

        if (_currentCustomer.AccountBalance < transferAmount)
        {
            Console.WriteLine("\n\t Insufficient funds.");
            return;
        }

        if (transferAmount < 10 || transferAmount > 10000)
        {
            Console.WriteLine("\n\t Invalid amount entered!\"Enter between 10 and 10000\" ");
            return;
        }

        _currentCustomer.AccountBalance -= transferAmount;

        receiver.AccountBalance += transferAmount;

        var transaction = new TransactionDetail
        {
            TransactionType = "Transfer",
            Sender = _currentCustomer.AccountNumber,
            Receiver = accountNumber,
            TransactionDate = DateTime.Now,
            TransactedAmount = transferAmount
        };

        context.Transactions.Add(transaction);
        context.SaveChanges();

        Console.WriteLine("\n\t Transfer successful.");
    }


    public void Withdraw(decimal amountToWithdraw)
    {
        var context = atmDbContextFactory.CreateDbContext(null);
        _atmTotalBalance = context.AtmMachine.Sum(a => a.AtmBalance);

        if (amountToWithdraw > _currentCustomer.AccountBalance)
        {
            Console.WriteLine("\n\t Insufficient account balance!");
            return;
        }

        if (amountToWithdraw > 10000 || amountToWithdraw < 10)
        {
            Console.WriteLine("\n\t Sorry! Invalid amount (Enter between 10 and 10000): ");
            return;
        }

        if (amountToWithdraw > _atmTotalBalance)
        {
            Console.WriteLine("\n\t Sorry! ATM machine unable to dispense cash, try again some other time.");
            return;
        }

        _currentCustomer.AccountBalance -= amountToWithdraw;
        _atmTotalBalance -= amountToWithdraw;

        TransactionDetail transaction = new TransactionDetail
        {
            TransactionType = "Withdrawal",
            Sender = $"{_currentCustomer.FirstName} {_currentCustomer.LastName}",
            Receiver = $"{_currentCustomer.AccountNumber}",
            TransactedAmount = amountToWithdraw,
            TransactionDate = DateTime.Now
        };

        context.Transactions.Add(transaction);
        context.SaveChanges();

        Console.WriteLine($"\n\t Withdrawal successful!. Your new balance is ${_currentCustomer.AccountBalance}.");
    }


    public void ChangeAtmPin(string currentPin)
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        if (currentPin != _currentCustomer.DefaultAtmPin)
        {
            Console.WriteLine("\n\t Invalid current ATM pin!");
            return;
        }

        Console.Write("\n\t Enter new ATM pin: \n\t ");
        string newPin = Console.ReadLine();

        Console.Write("\n\t Confirm new ATM pin: \n\t ");
        string confirmPin = Console.ReadLine();

        if (newPin != confirmPin)
        {
            Console.WriteLine("\n\t New ATM Pin Mis-matched! \n\t ");
            return;
        }

        _currentCustomer.NewAtmPin = newPin;
        _currentCustomer.DefaultAtmPin = newPin;
        _currentCustomer.IsActive = true;

        context.Customers.Update(_currentCustomer);
        context.SaveChanges();

        Console.WriteLine("\n\t ATM pin updated successfully!");
    }


    public void ViewAccountTransactions()
    {
        var context = atmDbContextFactory.CreateDbContext(null);

        var transactions = context.Transactions
                                .Where(t => t.Sender == $"{_currentCustomer.AccountNumber}"
                                            || t.Receiver == $"{_currentCustomer.AccountNumber}")
                                .OrderByDescending(t => t.TransactionDate).ToList();

        Console.WriteLine("\n\t Transaction History\n\t ====================\n\t ");

        Console.WriteLine($"\t {"Type",-10} {"Sender/Receiver",-20} {"Amount",-10} {"Date/Time",-20}");

        foreach (var transaction in transactions)
        {
            string transactionType = transaction.TransactionType;

            string senderOrReceiver = "";

            if (transaction.Sender == $"{_currentCustomer.AccountNumber}")
            {
                senderOrReceiver = $"To {transaction.Receiver}";
            }
            else
            {
                senderOrReceiver = $"From {transaction.Sender}";
            }

            Console.WriteLine($"\t {transactionType,-10} {senderOrReceiver,-20} ${transaction.TransactedAmount,-10} {transaction.TransactionDate,-20}");
        }
    }
}


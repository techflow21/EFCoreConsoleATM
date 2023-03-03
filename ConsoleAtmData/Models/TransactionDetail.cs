
namespace EFCoreATM_Data.Models;

public class TransactionDetail
{
    public int Id { get; set; }
    public string TransactionType { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public decimal TransactedAmount { get; set; }
    public DateTime TransactionDate { get; set; }

}

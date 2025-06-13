namespace ManagerMoney.Models;

public class TransactionUpdateViewModel: TransactionCreateViewModel
{
    public int LastAccountId { get; set; }
    public decimal LastAmount { get; set; }
    public string UrlReturn { get; set; }
}
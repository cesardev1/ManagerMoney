namespace ManagerMoney.Models;

public class TransactionUpdateViewModel: TransactionCreateViewModel
{
    public int LastAccountId { get; set; }
    public decimal LastMount { get; set; }
    public string UrlReturn { get; set; }
}
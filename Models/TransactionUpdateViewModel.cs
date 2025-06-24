namespace ManagerMoney.Models;

public class TransactionUpdateViewModel: TransactionCreateViewModel
{
    public int LastAccountId { get; set; }
    public decimal PreviousAmount { get; set; }
    public string UrlReturn { get; set; }
}
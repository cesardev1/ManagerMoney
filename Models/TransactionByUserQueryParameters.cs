namespace ManagerMoney.Models;

public class TransactionByUserQueryParameters
{
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
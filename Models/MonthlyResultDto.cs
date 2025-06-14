namespace ManagerMoney.Models;

public class MonthlyResultDto
{
    public int Month { get; set; }
    public DateTime BaseDate { get; set; }
    public decimal Amount { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public OperationType OperationTypeId { get; set; }
}
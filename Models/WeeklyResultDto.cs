namespace ManagerMoney.Models;

public class WeeklyResultDto
{
    public int Week { get; set; }
    public decimal Amount { get; set; }
    public OperationType OperationTypeId { get; set; }
    public decimal Incomes { get; set; }
    public decimal Expenses { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
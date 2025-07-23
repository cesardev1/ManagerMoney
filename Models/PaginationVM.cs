namespace ManagerMoney.Models;

public class PaginationVM
{
    public int PageIndex { get; set; } = 1;
    private int recordsPerPage = 10;
    private readonly int maximumRecordsPerPage = 50;

    public int RecordsPerPage
    {
        get => recordsPerPage;
        set => recordsPerPage = value > maximumRecordsPerPage ? maximumRecordsPerPage : value;
    }
    
    public int Offset => (PageIndex-1) * RecordsPerPage;

}
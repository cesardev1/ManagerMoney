namespace ManagerMoney.Models;

public class PaginationResponse
{
    public int PageIndex { get; set; }
    public int RecordsPerPage { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords/RecordsPerPage);
    public string BaseURL { get; set; }
}

public class PaginationResponse<T> : PaginationResponse
{
    public IEnumerable<T> Items { get; set; }
}
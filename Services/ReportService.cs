using ManagerMoney.Models;

namespace ManagerMoney.Services;

public interface IReportService
{
    Task<DetailedTransactionReport> GetTransactionReportPerAccount(int userId, int accountId, int month,
        int year, dynamic ViewBag);

    Task<DetailedTransactionReport> GetTransactionDetailReport(int userId, int month, int year, dynamic ViewBag);
    Task<IEnumerable<WeeklyResultDto>> GetTransactionReportPerWeek(int userId, int month, int year,dynamic ViewBag);
}

public class ReportService : IReportService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly HttpContext _httpContext;

    public ReportService(ITransactionRepository transactionRepository,IHttpContextAccessor httpContextAccessor)
    {
        _transactionRepository = transactionRepository;
        _httpContext = httpContextAccessor.HttpContext;
    }
    
    public async Task<DetailedTransactionReport> GetTransactionDetailReport(int userId, int month, int year, dynamic ViewBag)
    {
        var (startDate, endDate) = GenerateStartAndEndDate(month, year);
        
        var parameter = new TransactionByUserQueryParameters()
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        };
        var transactions = await _transactionRepository.GetAllByUserId(parameter);
        var model = GenerateDetailedTransactionReport(transactions, startDate, endDate);
        SetViewBagForTransactionReport(ViewBag, startDate);
        return model;
    }

    public async Task<IEnumerable<WeeklyResultDto>> GetTransactionReportPerWeek(int userId, int month, int year,dynamic ViewBag)
    {
        var (startDate, endDate) = GenerateStartAndEndDate(month, year);
        
        var parameter = new TransactionByUserQueryParameters()
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        };
        
        SetViewBagForTransactionReport(ViewBag, startDate);
        var model = await _transactionRepository.GetPerWeek(parameter);
        return model;
    }
    
    public async Task<DetailedTransactionReport> GetTransactionReportPerAccount(int userId, int accountId, int month,
        int year, dynamic ViewBag)
    {
        var (startDate, endDate) = GenerateStartAndEndDate(month, year);
        
        var getTransactionByAccount = new GetTransactionsByAccount()
        {
            AccountId = accountId,
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        };
        var transactions = await _transactionRepository.GetAllByAccountId(getTransactionByAccount);
        var model = GenerateDetailedTransactionReport(transactions, startDate, endDate);

        SetViewBagForTransactionReport(ViewBag, startDate);
        return model;
    }

    private void SetViewBagForTransactionReport(dynamic ViewBag, DateTime startDate)
    {
        ViewBag.LastMonth = startDate.AddMonths(-1).Month;
        ViewBag.LastYear = startDate.AddMonths(-1).Year;
        ViewBag.NextMonth = startDate.AddMonths(1).Month;
        ViewBag.NextYear = startDate.AddMonths(1).Year;
        ViewBag.urlReturn = _httpContext.Request.Path + _httpContext.Request.QueryString;
    }

    private static DetailedTransactionReport GenerateDetailedTransactionReport(IEnumerable<Transaction> transactions, DateTime startDate,
        DateTime endDate)
    {
        var model = new DetailedTransactionReport();

        var transactionsPerDate = transactions.OrderByDescending(x => x.TransactionDate)
            .GroupBy(x => x.TransactionDate)
            .Select(group => new DetailedTransactionReport.TransactionPerDate()
            {
                TransactionDate = group.Key,
                Transactions = group.AsEnumerable()
            });
        
        model.GroupedTransactions = transactionsPerDate;
        model.StartDate = startDate;
        model.EndDate = endDate;
        return model;
    }

    private (DateTime startDate, DateTime endDate) GenerateStartAndEndDate(int month, int year)
    {
        DateTime startDate;
        DateTime endDate;

        if (month <= 0 || month > 12 || year <= 1900)
        {
            var today = DateTime.Today;
            startDate = new DateTime(today.Year, today.Month, 1);
        }
        else
        {
            startDate = new DateTime(year, month, 1);
        }
        endDate = startDate.AddMonths(1).AddDays(-1);

        return (startDate, endDate);
    }
}
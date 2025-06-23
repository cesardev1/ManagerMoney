using Dapper;
using ManagerMoney.Models;
using Microsoft.Data.SqlClient;

namespace ManagerMoney.Services;

public interface ITransactionRepository
{
    Task Create(Transaction transaction);
    Task<Transaction> GetById(int id, int userId);
    Task Update(Transaction transaction, decimal lastAmount, int lastAccountId);
    Task Delete(int id);
    Task<IEnumerable<Transaction>> GetAllByAccountId(GetTransactionsByAccount model);
    Task<IEnumerable<WeeklyResultDto>> GetPerWeek(TransactionByUserQueryParameters model);
    Task<IEnumerable<Transaction>> GetAllByUserId(TransactionByUserQueryParameters model);
    Task<IEnumerable<MonthlyResultDto>> GetPerMonth(int userId, int year);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly SecretsOptions _secretOptions;

    public TransactionRepository(SecretsOptions secretOptions)
    {
        _secretOptions = secretOptions;
    }

    public async Task Create(Transaction transaction)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        var id = await connection.QuerySingleAsync<int>("Transactions_Insert",
            new
            {
                transaction.UserId,
                transaction.TransactionDate,
                transaction.Amount,
                transaction.CategoryId,
                transaction.AccountId,
                transaction.Note
            }, commandType: System.Data.CommandType.StoredProcedure);

        transaction.Id = id;
    }

    public async Task Update(Transaction transaction, decimal previousAmount, int previousAccountId)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        await connection.ExecuteAsync("Transactions_Update", new
            {
                transaction.Id,
                transaction.TransactionDate,
                transaction.Amount,
                transaction.CategoryId,
                transaction.AccountId,
                transaction.Note,
                previousAmount,
                previousAccountId,
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<Transaction>> GetAllByAccountId(GetTransactionsByAccount model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<Transaction>(@"SELECT t.Id,t.Amount,t.TransactionDate,c.Name as Category
                                                              FROM [Transactions] t
                                                              INNER JOIN Categories  c
                                                              ON c.Id = t.CategoryId
                                                              INNER JOIN Accounts  a
                                                              ON a.Id = t.AccountId
                                                              WHERE t.AccountId = @AccountId AND t.UserId = @UserId
                                                              AND TransactionDate BETWEEN @StartDate AND @EndDate",
            model);
    }

    public async Task<IEnumerable<WeeklyResultDto>> GetPerWeek(TransactionByUserQueryParameters model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<WeeklyResultDto>(
            @"SELECT DATEDIFF(d, @StartDate, TransactionDate)/7+1 as Week, SUM(Amount) as Amount, cat.OperationTypeId
                                                                FROM [Transactions]
                                                                INNER JOIN [Categories] cat
                                                                ON cat.Id = [Transactions].CategoryId
                                                                WHERE [Transactions].UserId = @UserId AND
                                                                TransactionDate BETWEEN @StartDate AND @EndDate
                                                                Group By DATEDIFF(d, @StartDate, TransactionDate)/7, cat.OperationTypeId"
            , model);
    }

    public async Task<IEnumerable<MonthlyResultDto>> GetPerMonth(int userId, int year)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<MonthlyResultDto>(@"SELECT MONTH(TransactionDate) as Month,
                                                                   SUM(Amount) as Amount, cat.OperationTypeId
                                                                   FROM [Transactions]
                                                                   INNER JOIN Categories cat
                                                                   ON cat.Id = [Transactions].CategoryId
                                                                   WHERE [Transactions].UserId = @userId AND YEAR([Transactions].TransactionDate) = @Year
                                                                   GROUP BY MONTH(TransactionDate), cat.OperationTypeId;", new { userId, year });
    }

    public async Task<IEnumerable<Transaction>> GetAllByUserId(TransactionByUserQueryParameters model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<Transaction>(@"SELECT t.Id,t.Amount,t.TransactionDate,c.Name as Category, a.Name as Account, c.OperationTypeId, Note
                                                              FROM [Transactions] t
                                                              INNER JOIN Categories  c
                                                              ON c.Id = t.CategoryId
                                                              INNER JOIN Accounts  a
                                                              ON a.Id = t.AccountId
                                                              WHERE t.UserId = @UserId
                                                              AND TransactionDate BETWEEN @StartDate AND @EndDate
                                                              ORDER BY t.TransactionDate DESC",model);
    }

    public async Task<Transaction> GetById(int id, int userId)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryFirstOrDefaultAsync<Transaction>(
            @"SELECT [Transactions].*, cat.OperationTypeId
                  From [Transactions]
                  INNER JOIN Categories as cat
                  ON cat.Id = [Transactions].CategoryId
                  WHERE [Transactions].Id = @Id AND [Transactions].UserId = @UserId",
            new { id, userId });
    }

    public async Task Delete(int id)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        await connection.ExecuteAsync(@"Transactions_Delete", 
                                new { id },
                                commandType: System.Data.CommandType.StoredProcedure);
        
    }
}
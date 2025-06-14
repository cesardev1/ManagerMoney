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
        var id = await connection.QuerySingleAsync<int>("Transaction_Insert",
            new
            {

                transaction.UserId,
                transaction.DateTransaction,
                Amount = transaction.Amount,
                transaction.CategoryId,
                transaction.AccountId,
                transaction.Note
            }, commandType: System.Data.CommandType.StoredProcedure);

        transaction.Id = id;
    }

    public async Task Update(Transaction transaction, decimal lastAmount, int lastAccountId)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        await connection.ExecuteAsync("Transaction_Update", new
            {
                transaction.Id,
                transaction.DateTransaction,
                Mount = transaction.Amount,
                transaction.CategoryId,
                transaction.AccountId,
                transaction.Note,
                lastAmount,
                lastAccountId,
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }



    public async Task<IEnumerable<Transaction>> GetAllByAccountId(GetTransactionsByAccount model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<Transaction>(@"SELECT t.Id,t.Amount,t.DateTransaction,c.Name as Category
                                                              FROM [Transaction] t
                                                              INNER JOIN Categories  c
                                                              ON c.Id = t.CategoryId
                                                              INNER JOIN Account  a
                                                              ON a.Id = t.AccountId
                                                              WHERE t.AccountId = @AccountId AND t.UserId = @UserId
                                                              AND DateTransaction BETWEEN @StartDate AND @EndDate",
            model);
    }

    public async Task<IEnumerable<WeeklyResultDto>> GetPerWeek(TransactionByUserQueryParameters model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<WeeklyResultDto>(
            @"SELECT DATEDIFF(d, @StartDate, DateTransaction)/7+1 as Week, SUM(Amount) as Amount, cat.OperationTypeId
                                                                FROM [ManejoPresupuesto].[dbo].[Transaction]
                                                                INNER JOIN [Categories] cat
                                                                ON cat.Id = [Transaction].CategoryId
                                                                WHERE [Transaction].UserId = @UserId AND
                                                                DateTransaction BETWEEN @StartDate AND @EndDate
                                                                Group By DATEDIFF(d, @StartDate, DateTransaction)/7, cat.OperationTypeId"
            , model);
    }

    public async Task<IEnumerable<MonthlyResultDto>> GetPerMonth(int userId, int year)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<MonthlyResultDto>(@"SELECT MONTH(DateTransaction) as Month,
                                                                   SUM(Amount) as Amount, cat.OperationTypeId
                                                                   FROM [Transaction]
                                                                   INNER JOIN Categories cat
                                                                   ON cat.Id = [Transaction].CategoryId
                                                                   WHERE [Transaction].UserId = @userId AND YEAR([Transaction].DateTransaction) = @Year
                                                                   GROUP BY MONTH(DateTransaction), cat.OperationTypeId;", new { userId, year });
    }

public async Task<IEnumerable<Transaction>> GetAllByUserId(TransactionByUserQueryParameters model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<Transaction>(@"SELECT t.Id,t.Amount,t.DateTransaction,c.Name as Category, a.Name as Account, c.OperationTypeId, Note
                                                              FROM [Transaction] t
                                                              INNER JOIN Categories  c
                                                              ON c.Id = t.CategoryId
                                                              INNER JOIN Account  a
                                                              ON a.Id = t.AccountId
                                                              WHERE t.UserId = @UserId
                                                              AND DateTransaction BETWEEN @StartDate AND @EndDate
                                                              ORDER BY t.DateTransaction DESC",model);
    }
    public async Task<Transaction> GetById(int id, int userId)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryFirstOrDefaultAsync<Transaction>(
            @"SELECT [Transaction].*, cat.OperationTypeId
                  From [Transaction]
                  INNER JOIN Categories as cat
                  ON cat.Id = [Transaction].CategoryId
                  WHERE [Transaction].Id = @Id AND [Transaction].UserId = @UserId",
            new { id, userId });
    }

    public async Task Delete(int id)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        await connection.ExecuteAsync(@"Transaction_Delete", 
                                new { id },
                                commandType: System.Data.CommandType.StoredProcedure);
        
    }
}
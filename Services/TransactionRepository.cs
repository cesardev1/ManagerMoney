using Dapper;
using ManagerMoney.Models;
using Microsoft.Data.SqlClient;

namespace ManagerMoney.Services;

public interface ITransactionRepository
{
    Task Create(Transaction transaction);
    Task<Transaction> GetById(int id, int userId);
    Task Update(Transaction transaction, decimal LastMount, int LastAccountId);
    Task Delete(int id);
    Task<IEnumerable<Transaction>> GetAllByAccountId(GetTransactionsByAccount model);
    Task<IEnumerable<Transaction>> GetAllByUserId(TransactionByUserQueryParameters model);
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
                transaction.Mount,
                transaction.CategoryId,
                transaction.AccountId,
                transaction.Note
            }, commandType: System.Data.CommandType.StoredProcedure);
        
        transaction.Id = id;
    }

    public async Task Update(Transaction transaction, decimal LastMount, int LastAccountId)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        await connection.ExecuteAsync("Transaction_Update", new
            {
                transaction.Id,
                transaction.DateTransaction,
                transaction.Mount,
                transaction.CategoryId,
                transaction.AccountId,
                transaction.Note,
                LastMount,
                LastAccountId,
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<Transaction>> GetAllByAccountId(GetTransactionsByAccount model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<Transaction>(@"SELECT t.Id,t.Mount,t.DateTransaction,c.Name as Category
                                                              FROM [Transaction] t
                                                              INNER JOIN Categories  c
                                                              ON c.Id = t.CategoryId
                                                              INNER JOIN Account  a
                                                              ON a.Id = t.AccountId
                                                              WHERE t.AccountId = @AccountId AND t.UserId = @UserId
                                                              AND DateTransaction BETWEEN @StartDate AND @EndDate",model);
    }
    public async Task<IEnumerable<Transaction>> GetAllByUserId(TransactionByUserQueryParameters model)
    {
        using var connection = new SqlConnection(_secretOptions.ConnectionString);
        return await connection.QueryAsync<Transaction>(@"SELECT t.Id,t.Mount,t.DateTransaction,c.Name as Category
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
using Dapper;
using ManagerMoney.Models;
using Microsoft.Data.SqlClient;

namespace ManagerMoney.Services;

public interface IAccountRepository
{
    Task Create(Account account);
    Task<IEnumerable<Account>> FindByUserId(int userId);
    Task<Account> GetById(int id, int userId);
    Task Update(AccountCreateViewModel account);
    Task Delete(int id);
}

public class AccountRepository: IAccountRepository
{
    private readonly SecretsOptions _secretsOptions;

    public AccountRepository(SecretsOptions secretsOptions)
    {
        _secretsOptions = secretsOptions;
    }

    public async Task Create(Account account)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        var id = await connection.QuerySingleAsync<int>(
            @"INSERT INTO Accounts (Name,AccountTypeId,Description,Balance)
                  VALUES (@Name, @AccountTypeId, @Description, @Balance);
                  SELECT SCOPE_IDENTITY();",account);
        
        account.Id = id;
    }

    public async Task<IEnumerable<Account>> FindByUserId(int userId)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        return await connection.QueryAsync<Account>(@"SELECT Accounts.id, Accounts.name, Balance, Description, aT.Name AS AccountType
                                                        FROM Accounts
                                                        INNER JOIN accountsType aT 
                                                        ON aT.Id = Accounts.AccountTypeId
                                                        WHERE aT.UserId = @UserId
                                                        ORDER BY aT.OrderIndex",new {userId});
                                                                        
    }

    public async Task<Account> GetById(int id, int userId)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        
        return await connection.QueryFirstOrDefaultAsync<Account>(
            @"SELECT Accounts.Id, Accounts.Name,Balance, Accounts.AccountTypeId
                FROM Accounts
                INNER JOIN accountsType aT 
                ON at.Id = Accounts.AccountTypeId
                WHERE aT.UserId = @UserId AND Accounts.Id = @Id", new { id, userId });
    }

    public async Task Update(AccountCreateViewModel account)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        await connection.ExecuteAsync(@"UPDATE Accounts
                                            SET Name=@Name, AccountTypeId=@AccountTypeId, Description=@Description, Balance=@Balance
                                            WHERE Id = @Id",account);
    }

    public async Task Delete(int id)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString); 
        await connection.ExecuteAsync(@"DELETE Account WHERE Id = @Id", new { id });
    }
}
using Dapper;
using ManagerMoney.Models;
using Microsoft.Data.SqlClient;

namespace ManagerMoney.Services;

public interface ICategoriesRepository
{
    Task<IEnumerable<Category>> GetAll(int userId);
    Task Create(Category category);
    Task<Category> GetById(int id, int userId);
    Task Update(Category category);
    Task Delete(int id);
}

public class CategoriesRepository: ICategoriesRepository
{
    private readonly SecretsOptions _secretsOptions;

    public CategoriesRepository(SecretsOptions secretsOptions)
    {
        _secretsOptions = secretsOptions;
    }

    public async Task<IEnumerable<Category>> GetAll(int userId)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        return await connection.QueryAsync<Category>(@"SELECT *
                                                        FROM Categories
                                                        WHERE UserId = @UserId", new {userId});
    }
    
    public async Task Create(Category category)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categories (Name, OperationTypeId, UserId)
                                                            Values(@Name, @OperationTypeId, @UserId)
                                                            SELECT SCOPE_IDENTITY()"
                                                            ,category);
        category.Id = id;
    }

    public async Task<Category> GetById(int id, int userId)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        return await connection.QueryFirstOrDefaultAsync<Category>(@"SELECT *
                                                                    FROM Categories
                                                                    WHERE  Id = @Id AND UserId = @UserId", 
                                                              new {id, userId});
    }

    public async Task Update(Category category)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        await connection.ExecuteAsync(@"UPDATE Categories
                                            SET Name=@Name, OperationTypeId=@OperationTypeId
                                            WHERE Id = @Id ",category);
    }
    
    public async Task Delete(int id)
    {
        using var connection = new SqlConnection(_secretsOptions.ConnectionString);
        await connection.ExecuteAsync(@"DELETE Categories WHERE Id = @Id", new {id});
    }
}
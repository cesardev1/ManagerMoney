using Dapper;
using ManagerMoney.Models;
using Microsoft.Data.SqlClient;

namespace ManagerMoney.Services
{
    public class UsersRepository : IUsersRepository
    {
        private readonly SecretsOptions _secretsOptions;

        public UsersRepository(SecretsOptions secretsOptions)
        {
            _secretsOptions = secretsOptions;
        }

        public async Task<int> CreateUser(User user)
        {
            using var connection = new SqlConnection(_secretsOptions.ConnectionString);
            var userId = await connection.QuerySingleAsync<int>(@"INSERT INTO Users (Email, NormalizedEmail, PasswordHash) 
                                                                VALUES (@Email, @NormalizedEmail, @PasswordHash);
                                                                SELECT SCOPE_IDENTITY();",user);

            await connection.ExecuteAsync("CreateDataNewUser", new { userId },
                commandType: System.Data.CommandType.StoredProcedure);

            return userId;
        }

        public async Task<User> GetUserByEmail(string normalizedEmail)
        {
            using var connection = new SqlConnection(_secretsOptions.ConnectionString);
            return await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE NormalizedEmail = @NormalizedEmail",new { normalizedEmail });
        }
    }

    public interface IUsersRepository
    {
        Task<int> CreateUser(User user);
        Task<User> GetUserByEmail(string normalizedEmail);
    }
}

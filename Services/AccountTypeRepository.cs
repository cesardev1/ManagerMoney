﻿using Dapper;
using ManagerMoney.Models;
using Microsoft.Data.SqlClient;

namespace ManagerMoney.Services
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountType accountType);
        Task<bool> Exist(string name, int userId);
        Task<IEnumerable<AccountType>> GetAll(int userId);
        Task<AccountType> GetById(int id, int userId);
        Task Update(AccountType accountType);
        Task Delete(int id);
        Task Order(IEnumerable<AccountType> accountTypes);
    }
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly SecretsOptions _secretOptions;

        public AccountTypeRepository(SecretsOptions secretsOptions)
        {
            _secretOptions = secretsOptions;
        }

        public async Task Create(AccountType accountType)
        {
            using var connection = new SqlConnection(_secretOptions.ConnectionString);
            var id = await connection.QuerySingleAsync<int>($@"AccountType_Insert",
                new {UserId= accountType.UserId, Name = accountType.Name},
                commandType: System.Data.CommandType.StoredProcedure);
            accountType.Id = id;
        }

        public async Task<bool> Exist(string name, int userId)
        {
            using var connection = new SqlConnection(_secretOptions.ConnectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 
                                                                        FROM accountType
                                                                        WHERE Name = @Name AND UserId = @UserId;",
                                                                        new { name, userId });

            return exist == 1;
        }

        public async Task<IEnumerable<AccountType>> GetAll(int userId)
        {
            using var connection = new SqlConnection(_secretOptions.ConnectionString);
            var accountTypes = await connection.QueryAsync<AccountType>(@"SELECT Id, Name, UserId, OrderBy
                                                                        FROM accountType
                                                                        WHERE UserId = @UserId
                                                                        ORDER BY OrderBy", new { userId });
            return accountTypes;
        }

        public async Task Update(AccountType accountType)
        {
            using var connection = new SqlConnection(_secretOptions.ConnectionString);
            await connection.ExecuteAsync(@"UPDATE accountType
                                            SET Name = @Name
                                            WHERE Id = @Id",accountType);
        }

        public async Task<AccountType> GetById(int id,int userId)
        {
            using var connection = new SqlConnection(_secretOptions.ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>(@"SELECT Id, Name, OrderBy
                                                                            FROM accountType
                                                                            WHERE Id = @Id AND UserId = @UserId", new { id, userId} );
            
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(_secretOptions.ConnectionString);
            await connection.ExecuteAsync(@"DELETE accountType WHERE Id = @Id",new { id });
        }

        public async Task Order(IEnumerable<AccountType> accountTypes)
        {
            var query = "UPDATE accountType SET OrderBy = @OrderBy WHERE Id = @Id";
            using var connection = new SqlConnection(_secretOptions.ConnectionString);
            await connection.ExecuteAsync(query, accountTypes);
        }
    }
}

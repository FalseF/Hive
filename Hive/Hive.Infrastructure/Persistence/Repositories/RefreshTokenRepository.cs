using Dapper;
using Hive.Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly string _connectionString;

        public RefreshTokenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(RefreshToken token)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO RefreshTokens (UserId, TokenHash, ExpiresAt, CreatedAt) 
                    VALUES (@UserId, @TokenHash, @ExpiresAt, @CreatedAt)";
            await db.ExecuteAsync(sql, token);
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            using var db = new SqlConnection(_connectionString);
            return await db.QueryFirstOrDefaultAsync<RefreshToken>(
                "SELECT * FROM RefreshTokens WHERE TokenHash = @TokenHash", new { TokenHash = tokenHash });
        }

        public async Task DeleteAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            await db.ExecuteAsync("DELETE FROM RefreshTokens WHERE Id = @Id", new { Id = id });
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            using var db = new SqlConnection(_connectionString);
            await db.ExecuteAsync("DELETE FROM RefreshTokens WHERE UserId = @UserId", new { UserId = userId });
        }
    }

}

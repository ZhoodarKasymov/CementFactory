using System.Data;
using CementFactoryAdmin.Models;
using Dapper;

public class AuthRepository
{
    private readonly IDbConnection _dbConnection;

    public AuthRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    // Authenticate user by comparing plain text password
    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var sql = "SELECT * FROM users WHERE username = @Username AND password = @Password";
        var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username, Password = password });

        return user;
    }

    // Check if user has a specific role (admin or operationist)
    public async Task<bool> UserHasRoleAsync(int userId, string roleName)
    {
        var sql = @"SELECT COUNT(1) 
                        FROM user_roles ur 
                        JOIN roles r ON ur.role_id = r.id 
                        WHERE ur.user_id = @UserId AND r.role_name = @RoleName";

        return await _dbConnection.ExecuteScalarAsync<bool>(sql, new { UserId = userId, RoleName = roleName });
    }

    // Retrieve roles for a given user
    public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
    {
        var sql = @"
            SELECT r.role_name 
            FROM roles r
            JOIN user_roles ur ON r.id = ur.role_id
            WHERE ur.user_id = @UserId";

        return await _dbConnection.QueryAsync<string>(sql, new { UserId = userId });
    }
}
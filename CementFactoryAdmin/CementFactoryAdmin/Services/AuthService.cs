using CementFactoryAdmin.Models;

namespace CementFactoryAdmin.Services;

public class AuthService
{
    private readonly AuthRepository _authRepository;

    public AuthService(AuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    // Log in the user and return their roles
    public async Task<User> LoginAsync(string username, string password)
    {
        var user = await _authRepository.AuthenticateUserAsync(username, password);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        return user;
    }

    // Check if user has a specific role
    public async Task<bool> UserHasRoleAsync(int userId, string roleName)
    {
        var roles = await _authRepository.GetUserRolesAsync(userId);
        return roles.Contains(roleName);
    }
}
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Repositories.interfaces;
using LibraryManagementSystem.Services.interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services;
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        return await _accountRepository.AuthenticateAsync(email, HashPassword(password));
    }

    public async Task<bool> RegisterAsync(User user, string password)
    {
        if (await _accountRepository.UserExistsAsync(user.Email))
            return false;

        // Optionally validate phone number format here if needed
        user.PasswordHash = HashPassword(password);
        return await _accountRepository.RegisterAsync(user);
    }


    public async Task<User?> GetByIdAsync(int id)
    {
        return await _accountRepository.GetByIdAsync(id);
    }

    // Simple SHA256 hash for demonstration (use a stronger hash in production)
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}


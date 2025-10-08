using LibraryManagementSystem.Entities;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Interfaces;

public interface IAccountService
{
	Task<User?> AuthenticateAsync(string email, string password);
	Task<bool> RegisterAsync(User user, string password);
	Task<User?> GetByIdAsync(int id);
}


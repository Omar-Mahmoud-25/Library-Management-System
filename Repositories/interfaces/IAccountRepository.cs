using LibraryManagementSystem.Entities;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repositories.interfaces
{
	public interface IAccountRepository
	{
		Task<User?> AuthenticateAsync(string email, string passwordHash);
	    Task<bool> RegisterAsync(User user);
		Task<bool> UserExistsAsync(string email);
		Task<User?> GetByIdAsync(int id);
	}
}

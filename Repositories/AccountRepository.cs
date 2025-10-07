using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagementSystem.Repositories;

class AccountRepository(LMSContext context) : IAccountRepository
{
    private readonly LMSContext _context = context;
    public async Task<User?> AuthenticateAsync(string email, string passwordHash)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> RegisterAsync(User user)
    {
        await _context.Users.AddAsync(user);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
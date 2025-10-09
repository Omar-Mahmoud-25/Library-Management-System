using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LibraryManagementSystem.Services.Interfaces;

public class UserService : IUserService
{
    private readonly LMSContext _context;

    public UserService(LMSContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.Users
            .AsNoTracking()
            .ToList();
    }
    
    public bool DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        _context.SaveChanges();
        return true;
    }

    public bool ToggleAdmin(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return false;

        user.IsAdmin = !user.IsAdmin;
        _context.SaveChanges();
        return true;
    }
    public User? GetUserById(int id)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == id);
    }


    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Borrowings)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserProfileViewModel?> GetUserProfileAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.Borrowings)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;

        return new UserProfileViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            JoiningDate = user.JoiningDate,
            IsAdmin = user.IsAdmin,
            IsActive = user.IsActive,
            TotalBorrowings = user.Borrowings.Count(),
            ActiveBorrowings = user.Borrowings.Count(b => !b.IsReturned)
        };
    }

    public async Task<bool> UpdateUserProfileAsync(UserProfileViewModel model)
    {
        try
        {
            var user = await _context.Users.FindAsync(model.Id);
            if (user == null) return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Verify current password
            if (user.PasswordHash != HashPassword(currentPassword))
                return false;

            // Hash new password
            user.PasswordHash = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
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

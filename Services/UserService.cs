using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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


}

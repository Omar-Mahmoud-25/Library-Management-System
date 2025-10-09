using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services.Interfaces;

public interface IUserService
{
    List<User> GetAllUsers();
    bool DeleteUser(int id);
    bool ToggleAdmin(int id);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> UpdateUserProfileAsync(UserProfileViewModel model);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<UserProfileViewModel?> GetUserProfileAsync(int userId);
}


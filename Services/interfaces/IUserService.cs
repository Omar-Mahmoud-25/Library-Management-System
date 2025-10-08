using LibraryManagementSystem.Entities;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services.Interfaces;

public interface IUserService
{
    List<User> GetAllUsers();
    bool DeleteUser(int id);
    bool ToggleAdmin(int id);
}


using LibraryManagementSystem.Entities;
using System.Collections.Generic;

public interface IUserService
{
    List<User> GetAllUsers();
    bool DeleteUser(int id);
    bool ToggleAdmin(int id);
}


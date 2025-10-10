using LibraryManagementSystem.Entities;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services.Interfaces;

public interface IBorrowingService
{
    Task<List<Borrowing>> GetAllBorrowingsAsync();
    Task<int> GetBorrowedCountAsync();
    Task<int> GetOverdueCountAsync();
    Task<int> GetReturnedCountAsync();
    Task<bool> BorrowBookAsync(int bookId, int userId);
    Task<bool> ReturnBookAsync(int bookId, int userId);
    Task<List<Borrowing>> GetUserBorrowingsAsync(int userId);
    Task<bool> CanUserBorrowBookAsync(int bookId, int userId);
    // Task<bool> IsBookOverdueAsync(int borrowingId);
    Task<Borrowing?> GetBorrowingByIdAsync(int borrowingId);
    Task<List<Borrowing>> GetBookBorrowingsAsync(int bookId);
}

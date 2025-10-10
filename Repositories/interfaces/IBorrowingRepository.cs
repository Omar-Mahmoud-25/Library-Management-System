    using LibraryManagementSystem.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;
namespace LibraryManagementSystem.Repositories.Interfaces;

public interface IBorrowingRepository
{
    Task<bool> BorrowBookAsync(int bookId, int userId);
    Task<bool> ReturnBookAsync(int bookId, int userId);
    Task<List<Borrowing>> GetUserBorrowingsAsync(int userId);
    Task<bool> CanUserBorrowBookAsync(int bookId, int userId);
    Task<bool> IsUserBorrowingBookAsync(int bookId, int userId);
    // Task<bool> IsBookOverdueAsync(int borrowingId);
    Task<Borrowing?> GetBorrowingByIdAsync(int borrowingId);
    Task<Borrowing?> GetBorrowingByBookAndUserAsync(int bookId, int userId);
    Task<List<Borrowing>> GetBookBorrowingsAsync(int bookId);
    Task<List<Borrowing>> GetAllBorrowingsAsync();
    Task<int> GetBorrowedCountAsync();
    Task<int> GetOverdueCountAsync();
    Task<int> GetReturnedCountAsync();
}

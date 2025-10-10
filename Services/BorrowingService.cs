using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.Repositories.Interfaces;

namespace LibraryManagementSystem.Services;

public class BorrowingService : IBorrowingService
{
    private readonly IBorrowingRepository _borrowingRepository;

    public BorrowingService(IBorrowingRepository borrowingRepository)
    {
        _borrowingRepository = borrowingRepository;
    }

    public async Task<bool> BorrowBookAsync(int bookId, int userId)
    {
        // Check if user can borrow the book first
        if (!await _borrowingRepository.IsUserBorrowingBookAsync(bookId, userId))
            return false;


        return await _borrowingRepository.BorrowBookAsync(bookId, userId);
    }

    public async Task<bool> CanUserBorrowBookAsync(int bookId, int userId)
    {
        return await _borrowingRepository.CanUserBorrowBookAsync(bookId, userId)
            && !await _borrowingRepository.IsUserBorrowingBookAsync(bookId, userId);
    }

    public async Task<List<Borrowing>> GetAllBorrowingsAsync()
    {
        return await _borrowingRepository.GetAllBorrowingsAsync();
    }

    public async Task<List<Borrowing>> GetBookBorrowingsAsync(int bookId)
    {
        return await _borrowingRepository.GetBookBorrowingsAsync(bookId);
    }

    public async Task<int> GetBorrowedCountAsync()
    {
        return await _borrowingRepository.GetBorrowedCountAsync();
    }

    public async Task<Borrowing?> GetBorrowingByIdAsync(int borrowingId)
    {
        return await _borrowingRepository.GetBorrowingByIdAsync(borrowingId);
    }

    public async Task<int> GetOverdueCountAsync()
    {
        return await _borrowingRepository.GetOverdueCountAsync();
    }

    public async Task<int> GetReturnedCountAsync()
    {
        return await _borrowingRepository.GetReturnedCountAsync();
    }

    public async Task<List<Borrowing>> GetUserBorrowingsAsync(int userId)
    {
        return await _borrowingRepository.GetUserBorrowingsAsync(userId);
    }

    // public async Task<bool> IsBookOverdueAsync(int borrowingId)
    // {
    //     return await _borrowingRepository.IsBookOverdueAsync(borrowingId);
    // }

    public async Task<bool> ReturnBookAsync(int bookId, int userId)
    {
        if (!await _borrowingRepository.IsUserBorrowingBookAsync(bookId, userId))
            return false;
        return await _borrowingRepository.ReturnBookAsync(bookId, userId);
    }
}

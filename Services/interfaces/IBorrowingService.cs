using LibraryManagementSystem.Entities;
using System.Collections.Generic;

public interface IBorrowingService
{
    List<Borrowing> GetAllBorrowings();
    int GetBorrowedCount();
    int GetOverdueCount();
    int GetReturnedCount();
}

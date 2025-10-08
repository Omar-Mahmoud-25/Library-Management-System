using LibraryManagementSystem.Entities;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services.Interfaces;

public interface IBorrowingService
{
    List<Borrowing> GetAllBorrowings();
    int GetBorrowedCount();
    int GetOverdueCount();
    int GetReturnedCount();
}

namespace LibraryManagementSystem.Repositories;

using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Context;
class BookRepository : IBookRepository
{
    private readonly LMSContext _context;

    public BookRepository(LMSContext context)
    {
        _context = context;
    }

    // Implement repository methods here
}
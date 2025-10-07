using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;

public interface IBookService
{
    Task<List<Book>> GetAll();
    Task Create(CreateBookViewModel model);
    Book? GetById(int id);
    Task<Book?> UpdateAsync(EditBookViewModel model);
    Task<bool> DeleteAsync(int id);

}

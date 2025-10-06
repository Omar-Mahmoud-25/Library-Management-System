using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;

public interface IBookService
{
    Task<List<Book>> GetAll();
    Task Create(CreateBookViewModel model);
    Book GetById(int id);
    Task Update(EditBookViewModel model);
    Task<bool> DeleteAsync(int id);

}

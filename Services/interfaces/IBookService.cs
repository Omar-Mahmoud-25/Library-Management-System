using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services.Interfaces;

public interface IBookService
{
    Task<List<Book>> GetAll(int? categoryId = null, string? search = null);
    Task Create(CreateBookViewModel model);
    Book? GetById(int id);
    Task<Book?> UpdateAsync(EditBookViewModel model);
    Task<bool> DeleteAsync(int id);
    string GetCoverImageUrl(string? fileName);
}

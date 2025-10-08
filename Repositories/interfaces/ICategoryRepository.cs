using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
    Task<bool> CategoryExistsAsync(string name);
}
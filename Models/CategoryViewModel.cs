using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class CategoryViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Books Count")]
    public int BooksCount { get; set; }
}

public class CreateCategoryViewModel
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }
}

public class EditCategoryViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }
}
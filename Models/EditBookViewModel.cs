using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class EditBookViewModel
{
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string Author { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public DateTime PublishedDate { get; set; }
    
    [Required]
    public int CopiesAvailable { get; set; }
    
    [Display(Name = "New Cover Image")]
    public IFormFile? CoverImageUrl { get; set; }
    
    public string? CurrentCoverImageUrl { get; set; }

    [Required]
    public List<int> SelectedCategoryIds { get; set; } = new();
    
    public List<SelectListItem> Categories { get; set; } = new();
}

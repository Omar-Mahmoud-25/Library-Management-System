using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class CreateBookViewModel
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        public int CopiesAvailable { get; set; } = 1;

        [Display(Name = "Cover Image")]
        public IFormFile? CoverImageUrl { get; set; }

        [Required]
        [Display(Name = "Categories")]
        public List<int> SelectedCategoryIds { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
    }
}

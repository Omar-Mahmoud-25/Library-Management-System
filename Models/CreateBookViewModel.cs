using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class CreateBookViewModel
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        public int CopiesAvailable { get; set; } = 1;

        [Required, Url]
        public string CoverImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Title { get; set; }
    [Required, MaxLength(100)]
    public string Author { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }
    [Required]
    public DateTime PublishedDate { get; set; }
    public int CopiesAvailable { get; set; } = 1;
    [Required, Url]
    public string CoverImageUrl { get; set; }

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    public virtual ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}
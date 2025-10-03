namespace LibraryManagementSystem.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class Borrowing
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required]
    public DateTime BorrowedDate { get; set; } = DateTime.Now;

    public DateTime? ReturnedDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [ForeignKey("BookId")]
    public Book Book { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [NotMapped]
    public bool IsReturned => ReturnedDate.HasValue;

    [NotMapped]
    public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;

    [NotMapped]
    public int DaysOverdue => IsOverdue ? (DateTime.Now - DueDate).Days : 0;
}
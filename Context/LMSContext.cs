using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Context;

public class LMSContext : DbContext
{
    public LMSContext(DbContextOptions<LMSContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookCategory>()
            .HasKey(bc => new { bc.BookId, bc.CategoryId });

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Borrowing>()
            .HasOne(b => b.User)
            .WithMany(u => u.Borrowings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Borrowing>()
            .HasOne(b => b.Book)
            .WithMany(bk => bk.Borrowings)
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

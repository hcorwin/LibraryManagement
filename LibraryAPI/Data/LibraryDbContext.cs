using LibraryAPI.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models;

public class LibraryDbContext : DbContext, ILibraryDbContext
{

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Available)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CheckedOutBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastCheckedOutDate).HasColumnType("datetime");
            entity.Property(e => e.LastReturnedDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4D62318C2").IsUnique();

            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

    }
}

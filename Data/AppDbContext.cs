using Microsoft.EntityFrameworkCore;

using NetUrlShortener.Entities;
using NetUrlShortener.Services;

namespace NetUrlShortener.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<ShortendUrl> ShortendUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortendUrl>(builder =>
        {
            //Preformanse
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);
            //Unikatan Indeks
            builder.HasIndex(s => s.Code).IsUnique();
        });
    }
}
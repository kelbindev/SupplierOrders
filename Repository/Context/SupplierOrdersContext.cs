using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context;
public class SupplierOrdersContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.Country)
            .WithMany(c => c.Suppliers)
            .HasForeignKey(s => s.CountryId);

        modelBuilder.Entity<UserRefreshToken>()
         .HasOne(rt => rt.User)
         .WithOne(u => u.RefreshToken)
         .HasForeignKey<UserRefreshToken>(rt => rt.UserId);
    }

    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRefreshToken> UsersRefreshToken { get; set; }
}

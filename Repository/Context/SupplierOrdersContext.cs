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
    }

    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Country> Countries { get; set; }
}

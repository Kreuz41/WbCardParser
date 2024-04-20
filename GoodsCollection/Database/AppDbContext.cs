using GoodsCollection.Database.Configurations;
using GoodsCollection.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsCollection.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<GoodModel> Goods { get; set; } = null!;
    public DbSet<ImageModel> Images { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GoodConfiguration());
        modelBuilder.ApplyConfiguration(new ImageConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
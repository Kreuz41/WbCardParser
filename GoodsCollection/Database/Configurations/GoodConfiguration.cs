using GoodsCollection.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodsCollection.Database.Configurations;

public class GoodConfiguration : IEntityTypeConfiguration<GoodModel>
{
    public void Configure(EntityTypeBuilder<GoodModel> builder)
    {
        builder.HasKey(g => g.Article);
        builder.HasIndex(g => g.Article).IsUnique();
    }
}
using GoodsCollection.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodsCollection.Database.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<ImageModel>
{
    public void Configure(EntityTypeBuilder<ImageModel> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        builder.HasOne(i => i.Good)
            .WithMany(g => g.Images)
            .HasForeignKey(i => i.GoodArticle);
    }
}
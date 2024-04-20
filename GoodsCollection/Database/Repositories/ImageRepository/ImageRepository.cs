using GoodsCollection.Database.Models;

namespace GoodsCollection.Database.Repositories.ImageRepository;

public class ImageRepository : IImageRepository
{
    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    private readonly AppDbContext _context;

    public async Task Create(IEnumerable<string> paths, int goodArticle)
    {
        var images = paths.Select(p => new ImageModel
        {
            Path = p,
            GoodArticle = goodArticle
        });
        
        await _context.Images.AddRangeAsync(images);
        await _context.SaveChangesAsync();
    }
}
namespace GoodsCollection.Database.Repositories.ImageRepository;

public interface IImageRepository
{
    Task Create(IEnumerable<string> paths, int goodArticle);
}
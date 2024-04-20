namespace GoodsCollection.Database.Models;

public class ImageModel
{
    public long Id { get; set; }
    public string Path { get; set; }
    public int GoodArticle { get; set; }
    public GoodModel Good { get; set; } = null!;
}
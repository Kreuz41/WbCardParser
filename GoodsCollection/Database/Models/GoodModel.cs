namespace GoodsCollection.Database.Models;

public class GoodModel
{
    public int Article { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Rate { get; set; }
    public string? RatesCount { get; set; }
    public string? Brand { get; set; }
    public string? Price { get; set; }
    public long CreatedBy { get; set; }
    public int Status { get; set; }
    public ICollection<ImageModel> Images { get; set; } = new List<ImageModel>();
}
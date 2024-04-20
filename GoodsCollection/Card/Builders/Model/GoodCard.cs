namespace GoodsCollection.Card.Builders.Model;

public class GoodCard
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Rate { get; set; }
    public string? RatesCount { get; set; }
    public int? Article { get; set; }
    public string? Brand { get; set; }
    public string? Price { get; set; }
    public IEnumerable<string>? Images { get; set; } = [];
}
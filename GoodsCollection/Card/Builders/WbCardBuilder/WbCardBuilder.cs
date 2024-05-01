using GoodsCollection.Card.Builders.Model;
using GoodsCollection.Card.Parsers.WbParser;

namespace GoodsCollection.Card.Builders.WbCardBuilder;

public class WbCardBuilder
{
    public GoodCard? CreateCard(string article)
    {
        using var wbParser = new WbParser();
        if (!wbParser.Connect(article) || !wbParser.IsExist()) return null;
        
        var card = new GoodCard
        {
            Name = wbParser.GetName(),
            Article = wbParser.GetArticle(),
            Rate = wbParser.GetRate(),
            Price = "wbParser.GetPrice()",
            RatesCount = wbParser.GetRatesCount(),
            Brand = wbParser.GetBrand(),
        };
        
        var desc = wbParser.GetDescription();
        if (desc is null)
            return null;

        var sentence = desc.Split('.');
        if (sentence.Length < 3)
            return null;

        card.Description = string.Join('.', sentence[..2].ToList()) + ".";

        card.Images = wbParser.GetImages();

        return card;
    } 
}
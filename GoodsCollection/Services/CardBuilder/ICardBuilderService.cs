namespace GoodsCollection.Services.CardBuilder;

public interface ICardBuilderService
{
    void CardReceived(int article, long sender);
}
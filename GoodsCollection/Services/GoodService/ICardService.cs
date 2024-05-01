using GoodsCollection.Card.Builders.Model;
using GoodsCollection.Enums;

namespace GoodsCollection.Services.GoodService;

public interface ICardService
{
    event Action<int, long>? CardSaved;
    event Action<int, CardStatus>? StatusChanged;
    Task SaveCard(GoodCard card, long createdBy);
    void ChangeCardStatus(int article, CardStatus status);
    int GetQueueLength();
    GoodCard? GetNextCard();
    Task<bool> IsCardExist(int article);
    Task UploadCards();
}
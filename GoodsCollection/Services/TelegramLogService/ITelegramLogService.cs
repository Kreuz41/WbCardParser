using GoodsCollection.Enums;

namespace GoodsCollection.Services.TelegramLogService;

public interface ITelegramLogService
{
    void ArticleReceivedHandler(int article, long sender);
    void StatusChangedHandler(int article, CardStatus status);
    void CardSavedHandler(int article, long sender);
}
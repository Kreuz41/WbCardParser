using GoodsCollection.Card.Builders.Model;
using GoodsCollection.Enums;

namespace GoodsCollection.Telegram.Bot.TelegramService;

public interface ITelegramService
{
    event Action<int, long>? ArticleReceived;
    event Action<int, CardStatus>? StatusChanged;
    
    void Start();
    Task LogAction(string message, CancellationToken token = default);
    Task ApplyCard(long chatId, GoodCard card, CancellationToken token = default);
    Task PublishCard(GoodCard card, CancellationToken token = default, long chatId = 0);
    Task SendMessage(long chatId, string message, CancellationToken token = default);
}
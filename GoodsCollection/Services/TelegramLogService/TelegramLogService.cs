using GoodsCollection.Enums;
using GoodsCollection.Telegram.Bot;
using GoodsCollection.Telegram.Bot.TelegramService;

namespace GoodsCollection.Services.TelegramLogService;

public class TelegramLogService : ITelegramLogService
{
    public TelegramLogService(ITelegramService telegramService)
    {
        _telegramService = telegramService;
    }

    private readonly ITelegramService _telegramService;
    
    public async void ArticleReceivedHandler(int article, long sender)
    {
        await _telegramService.LogAction($"Card with article {article} received");
    }
    
    public async void StatusChangedHandler(int article, CardStatus status)
    {
        await _telegramService.LogAction($"Card with article {article} change status: {status}");
    }
    
    public async void CardSavedHandler(int article, long sender)
    {
        await _telegramService.LogAction($"Card with article {article} has been saved by {sender}");
    }
}
using GoodsCollection.Services.GoodService;
using GoodsCollection.Telegram.Bot;
using GoodsCollection.Telegram.Settings;

namespace GoodsCollection.Services.SchedulerService;

public class SchedulerMessagesService : BackgroundService
{
    public SchedulerMessagesService(ICardService service, ITelegramService telegramService)
    {
        _service = service;
        _telegramService = telegramService;
    }

    private readonly ICardService _service;
    private readonly ITelegramService _telegramService;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            if (DateTime.UtcNow.Minute != 0) continue;
            
            var card = _service.GetNextCard();
            if(card is not null)
                await _telegramService.SendCard(ChannelType.MainChannel, card, stoppingToken);
        }
    }
}
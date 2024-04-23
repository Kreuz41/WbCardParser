using GoodsCollection.Card.Builders.Model;
using GoodsCollection.Enums;
using GoodsCollection.Telegram.Settings;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GoodsCollection.Telegram.Bot;

public class TelegramService : ITelegramService
{
    private readonly TelegramBotClient _client;
    private readonly ChannelType _channelType;

    public TelegramService(BotSettings settings)
    {
        _client = new TelegramBotClient(settings.Token);
        _channelType = new ChannelType
        {
            MainChannel = settings.MainChannel,
            LogChannel = settings.LogChannel
        };
    }

    public event Action<int, long>? ArticleReceived;  
    public event Action<int, CardStatus>? StatusChanged;

    private readonly List<long> _whiteList = [1113106194, 1057428180, 1410785002];

    public void Start()
    {
        _client.StartReceiving(UpdateHandler, PoolingErrorHandler, new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
            ThrowPendingUpdates = true
        });
    }

    private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.CallbackQuery is not null)
        {
            var callbackData = update.CallbackQuery.Data!;
            var data = callbackData.Split(' ');
            await _client.AnswerCallbackQueryAsync(update.CallbackQuery.Id, cancellationToken: token);
            
            await _client.DeleteMessageAsync(update.CallbackQuery.Message!.Chat.Id, 
                update.CallbackQuery.Message.MessageId, cancellationToken: token);
            await _client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id,  
                data[1] == CardStatus.Confirmed.ToString() ? $"{data[1]}\u2705" : $"{data[1]}\u274c", 
                cancellationToken: token);
            
            OnStatusChanged(Convert.ToInt32(data[0]), Enum.Parse<CardStatus>(data[1]));
            return;
        }

        var chatId = update.Message!.Chat.Id;
        if (!_whiteList.Contains(chatId))
            return;

        if (update.Message.Text == "/start")
        {
            var msg = await _client.SendTextMessageAsync(chatId, GetHelloMessage(), 
                cancellationToken: token, parseMode: ParseMode.Markdown);
            await _client.PinChatMessageAsync(chatId, msg.MessageId, cancellationToken: token);
            return;
        }
            
        var isSuccess = int.TryParse(update.Message.Text, out var article);
        if (!isSuccess)
        {
            await _client.SendTextMessageAsync(chatId, "Incorrect article", cancellationToken: token);
            return;
        }

        await _client.SendTextMessageAsync(chatId, "Wait for response", cancellationToken: token);
        OnArticleReceived(article, chatId);
    }

    private async Task PoolingErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        await LogAction(exception.Message + "\n\ntgex", token);
    }

    public async Task LogAction(string message, CancellationToken token = default)
    {
        await _client.SendTextMessageAsync(_channelType.LogChannel, GetLogMessage(message), cancellationToken: token);
    }
    

    public async Task ApplyCard(long chatId, GoodCard card, CancellationToken token = default)
    {
        await PublishCard(card, token, chatId);
        
        var replies = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Confirm", $"{card.Article} {CardStatus.Confirmed}"),
                InlineKeyboardButton.WithCallbackData("Reject", $"{card.Article} {CardStatus.Rejected}")
            }
        });
        await _client.SendTextMessageAsync(chatId, "Confirm or reject", 
            replyMarkup: replies, cancellationToken: token);
    }

    public async Task SendMessage(long chatId, string message, CancellationToken token = default)
    {
        await _client.SendTextMessageAsync(chatId, message, cancellationToken: token);
    }

    public async Task PublishCard(GoodCard card, CancellationToken token = default, long chatId = 0)
    {
        var inputMedia = new List<InputMediaPhoto>();
        foreach (var image in card.Images!)
        {
            inputMedia.Add(new InputMediaPhoto(new InputFileUrl(image)));
            if (inputMedia.Count == 1)
            {
                inputMedia[0].Caption = GetMessageText(card);
                inputMedia[0].ParseMode = ParseMode.Markdown;
            }

            if (inputMedia.Count == 9) break;
        }

        if(chatId == 0)
            await _client.SendMediaGroupAsync(_channelType.MainChannel, inputMedia, cancellationToken: token);
        else
            await _client.SendMediaGroupAsync(chatId, inputMedia, cancellationToken: token);
    }

    private string GetMessageText(GoodCard card)
    {
        return $"""
                  *{card.Name}*
                         
                  {card.Description}
                         
                  Артикул: `{card.Article}`
                  Бренд: `{card.Brand}`
                  Рейтинг: {card.Rate}⭐  _({card.RatesCount})_

                  Ссылка: 
                  https://www.wildberries.ru/catalog/{card.Article}/detail.aspx
                  """;
    }

    private string GetHelloMessage()
    {
        return $"""
               Добро пожаловать в автоматизированный парсер карточек с WB.
               
               Инструкция по работе:
               1. Скидывай в бота артикул карточки с WB. После этого тебе придет сообщение об ожидании.
               2. После обработки артикула, если он соответствует базовым критериям, то бот пришлет сообщение с подтверждением валидности поста (вам необходимо проанализировать содержимое), иначе придет сообщение о том что карточка не валидна.
               3. Если карточка проходит по критериям, то подтвердить корректность поста, иначе отклонить.
               
               Критерии отбора карточек:
               1. Визуально текст в посту выглядит приемлемо и нет никаких переходов в тексте
               Пример: Наз*вание тов*ара
               2. Ссылка внизу поста ведет на карточку с товаром
               3. Все поля заполнены корректно
               4. Пост соответствует структуре:
               >>> Фотографии
               {GetMessageText(new GoodCard
               {
                   Article = 123123123,
                   Brand = "Бренд",
                   RatesCount = "Количество отзывов",
                   Description = "Описание",
                   Name = "Название товара",
                   Price = "Цена",
                   Rate = "Рейтинг"
               })}
               """;
    }
    private string GetLogMessage(string message)
    {
        return $"""
                [{DateTime.Now}]

                {message}
                """;
    }

    protected virtual void OnStatusChanged(int article, CardStatus status)
    {
        StatusChanged?.Invoke(article, status);
    }
    protected virtual void OnArticleReceived(int article, long receiver)
    {
        ArticleReceived?.Invoke(article, receiver);
    }
}
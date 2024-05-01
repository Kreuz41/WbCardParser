﻿using GoodsCollection.Card.Builders.WbCardBuilder;
using GoodsCollection.Services.GoodService;
using GoodsCollection.Telegram.Bot;
using GoodsCollection.Telegram.Bot.TelegramService;

namespace GoodsCollection.Services.CardBuilder;

public class CardBuilderService : ICardBuilderService
{
    public CardBuilderService(ICardService cardService, ITelegramService telegramService)
    {
        _cardService = cardService;
        _telegramService = telegramService;
    }
    
    private readonly ICardService _cardService;
    private readonly ITelegramService _telegramService;

    public async void CardReceived(int article, long sender)
    {
        if (await _cardService.IsCardExist(article))
        {
            await _telegramService.SendMessage(sender, "Card already exist");
            return;
        }
        
        var wbBuilder = new WbCardBuilder();
        var card = wbBuilder.CreateCard(article.ToString());
        if (card is null)
        {
            await _telegramService.SendMessage(sender, "Invalid card. Try other");
            return;
        }
        
        await _cardService.SaveCard(card, sender);
        await _telegramService.ApplyCard(sender, card);
    }
}
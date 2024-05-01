using GoodsCollection.Card.Builders.Model;
using GoodsCollection.Database.Repositories.GoodRepository;
using GoodsCollection.Database.Repositories.ImageRepository;
using GoodsCollection.Enums;

namespace GoodsCollection.Services.GoodService;

public class CardService : ICardService
{
    private readonly List<GoodCard> _cards = [];

    private readonly ICardRepository _cardRepository;
    private readonly IImageRepository _imageRepository;

    public event Action<int, long>? CardSaved;
    public event Action<int, CardStatus>? StatusChanged;

    public CardService(ICardRepository cardRepository, IImageRepository imageRepository)
    {
        _cardRepository = cardRepository;
        _imageRepository = imageRepository;
    }

    public async Task SaveCard(GoodCard card, long createdBy)
    {
        var transaction = await _cardRepository.Create(card, createdBy);
        await _imageRepository.Create(card.Images!, card.Article!.Value);
        await transaction.CommitAsync();
        
        OnCardSaved(card.Article.Value, createdBy);
    }

    public int GetQueueLength()
    {
        return _cards.Count;
    }

    public async void ChangeCardStatus(int article, CardStatus status)
    {
        await _cardRepository.ChangeStatus(article, (int)status);
        if (status is CardStatus.Confirmed)
        {
            var card = await _cardRepository.GetByArticle(article);
            if (card is not null)
                _cards.Add(card);
        }
        
        OnStatusChanged(article, status);
    }

    public GoodCard? GetNextCard()
    {
        var card = _cards.FirstOrDefault();
        if (card is null) return card;
        
        _cards.Remove(card);
        ChangeCardStatus(card.Article!.Value, CardStatus.Published);

        return card;
    }

    public async Task<bool> IsCardExist(int article)
    {
        var card = _cards.FirstOrDefault(c => c.Article == article);
        if (card is not null) return true;
        card = await _cardRepository.GetByArticle(article);
        return card is not null;
    }

    public async Task UploadCards()
    {
        _cards.AddRange(await _cardRepository.UploadConfirmedCards());
    }

    protected virtual void OnCardSaved(int arg1, long arg2)
    {
        CardSaved?.Invoke(arg1, arg2);
    }
    protected virtual void OnStatusChanged(int arg1, CardStatus arg2)
    {
        StatusChanged?.Invoke(arg1, arg2);
    }
}
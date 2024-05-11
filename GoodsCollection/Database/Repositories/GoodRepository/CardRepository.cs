using GoodsCollection.Card.Builders.Model;
using GoodsCollection.Database.Models;
using GoodsCollection.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GoodsCollection.Database.Repositories.GoodRepository;

public class CardRepository : ICardRepository
{
    private readonly AppDbContext _context;

    public CardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IDbContextTransaction> Create(GoodCard card, long createdBy)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        var existingGood = await GetByArticle(Convert.ToInt32(card.Article));
        if (existingGood is not null)
        {
            return transaction;
        }
        await _context.Goods.AddAsync(new GoodModel
        {
            Article = Convert.ToInt32(card.Article),
            Name = card.Name,
            Brand = card.Brand,
            Description = card.Description,
            Price = card.Price,
            Rate = card.Rate,
            RatesCount = card.RatesCount,
            Status = (int)CardStatus.Waiting,
            CreatedBy = createdBy,
            OldPrice = card.OldPrice
        });

        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<GoodCard?> GetByArticle(int article)
    {
        var card = await _context.Goods.Include(c => c.Images)
            .FirstOrDefaultAsync(g => g.Article == article);
        return card is null ? null : new GoodCard
        {
            RatesCount = card.RatesCount,
            Rate = card.Rate,
            Article = card.Article,
            Brand = card.Brand,
            Name = card.Name,
            Description = card.Description,
            Images = card.Images.Select(i => i.Path),
            Price = card.Price,
            OldPrice = card.OldPrice
        };
    }

    public async Task ChangeStatus(int article, int status)
    {
        var card = await _context.Goods.FirstOrDefaultAsync(c => c.Article == article);
        if (card is null) return;
        card.Status = status;
        _context.Goods.Update(card);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GoodCard>> UploadConfirmedCards()
    {
        var res = await _context.Goods.Include(g => g.Images)
            .Where(g => g.Status == (int)CardStatus.Confirmed)
            .ToListAsync();
        
        var list = new List<GoodCard>();

        foreach (var model in res)
        {
            var card = await GetByArticle(model.Article);
            if (card is not null)
                list.Add(card);
        }

        return list;
    }
}
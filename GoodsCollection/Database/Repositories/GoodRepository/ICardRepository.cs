using GoodsCollection.Card.Builders.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace GoodsCollection.Database.Repositories.GoodRepository;

public interface ICardRepository
{
    Task<IDbContextTransaction> Create(GoodCard card, long createdBy);
    Task<GoodCard?> GetByArticle(int article);
    Task ChangeStatus(int article, int status);
    Task<IEnumerable<GoodCard>> UploadConfirmedCards();
}
namespace GoodsCollection.Telegram.Settings;

public class CommandContext
{
    public long ChatId { get; set; }
    public string UpdateText { get; set; } = null!;
}
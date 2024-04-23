namespace GoodsCollection.Telegram.Settings;

public class BotSettings
{
    public string Token { get; set; } = null!;
    public long MainChannel { get; set; }
    public long LogChannel { get; set; }
    public long StatisticChannel { get; set; }
}
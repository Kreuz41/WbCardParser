namespace GoodsCollection.Telegram.Bot.Input.Commands.CommandContext;

public class SlashCommandContext
{
    public long ChatId { get; set; }
    public string? Data { get; set; }
}
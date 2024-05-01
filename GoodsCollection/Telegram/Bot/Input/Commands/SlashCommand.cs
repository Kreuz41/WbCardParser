using GoodsCollection.Telegram.Bot.Input.Commands.CommandContext;
using GoodsCollection.Telegram.Bot.Input.Commands.Delegates;

namespace GoodsCollection.Telegram.Bot.Input.Commands;

public class SlashCommand
{
    private Filter CommandFilter { get; set; } = null!;
    private Handler? Handler { get; set; }

    public SlashCommand AddHandler(Handler handler)
    {
        Handler = handler;
        return this;
    }

    public void AddFilter(Filter filter) => CommandFilter = filter;

    public bool CanHandle(string command) => CommandFilter.Invoke(command);

    public void Handle(SlashCommandContext context) => Handler?.Invoke(context);
}
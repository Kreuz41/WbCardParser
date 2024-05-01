using GoodsCollection.Telegram.Bot.Input.Commands.CommandContext;
using GoodsCollection.Telegram.Bot.Input.Commands.Delegates;

namespace GoodsCollection.Telegram.Bot.Input.Commands.CommandHandler;

public class SlashCommandHandler : ISlashCommandHandler
{
    private static IList<SlashCommand> Commands { get; } = [];

    public SlashCommand RegisterCommand(Handler handler)
    {
        var command = new SlashCommand().AddHandler(handler);
        Commands.Add(command);
        return command;
    }

    public void HandleCommand(string command, SlashCommandContext context)
    {
        foreach (var slashCommand in Commands)
        {
            if(slashCommand.CanHandle(command))
                slashCommand.Handle(context);
        }
    }
}
using GoodsCollection.Telegram.Bot.Input.Commands.CommandContext;
using GoodsCollection.Telegram.Bot.Input.Commands.Delegates;

namespace GoodsCollection.Telegram.Bot.Input.Commands.CommandHandler;

public interface ISlashCommandHandler
{
    SlashCommand RegisterCommand(Handler handler);
    void HandleCommand(string command, SlashCommandContext context);
}
using GoodsCollection.Telegram.Bot.Input.Commands.CommandContext;

namespace GoodsCollection.Telegram.Bot.Input.Commands.Delegates;

public delegate Task Handler(SlashCommandContext context);
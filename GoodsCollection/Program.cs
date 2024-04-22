using GoodsCollection.Database;
using GoodsCollection.Database.Repositories.GoodRepository;
using GoodsCollection.Database.Repositories.ImageRepository;
using GoodsCollection.Services.CardBuilder;
using GoodsCollection.Services.GoodService;
using GoodsCollection.Services.SchedulerService;
using GoodsCollection.Services.TelegramLogService;
using GoodsCollection.Telegram.Bot;
using GoodsCollection.Telegram.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var botSettings = new BotSettings();
builder.Configuration.GetSection("Bot").Bind(botSettings);
builder.Services.AddSingleton(botSettings);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
}, ServiceLifetime.Singleton);

builder.Services.AddSingleton<ITelegramService, TelegramService>();
builder.Services.AddSingleton<ICardService, CardService>();
builder.Services.AddSingleton<ICardService, CardService>();
builder.Services.AddSingleton<ICardBuilderService, CardBuilderService>();
builder.Services.AddSingleton<ITelegramLogService, TelegramLogService>();

builder.Services.AddSingleton<IImageRepository, ImageRepository>();
builder.Services.AddSingleton<ICardRepository, CardRepository>();

builder.Services.AddHostedService<SchedulerMessagesService>();

var app = builder.Build();

var bot = app.Services.GetService<ITelegramService>();
var goodService = app.Services.GetService<ICardService>();
var cardBuilder = app.Services.GetService<ICardBuilderService>();
var logService = app.Services.GetService<ITelegramLogService>();

using var scope = app.Services.CreateScope();
var conn = scope.ServiceProvider.GetRequiredService<AppDbContext>();

await conn.Database.MigrateAsync();

bot!.Start();
await goodService!.UploadCards();

bot.StatusChanged += goodService!.ChangeCardStatus;
bot.ArticleReceived += cardBuilder!.CardReceived;
bot.ArticleReceived += logService!.ArticleReceivedHandler;
goodService.StatusChanged += logService.StatusChangedHandler;
goodService.CardSaved += logService.CardSavedHandler;

app.Run();

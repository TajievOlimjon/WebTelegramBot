using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using WebApiTelegramBot;
using WebApiTelegramBot.BackgroundServices;
using WebApiTelegramBot.Data;
using WebApiTelegramBot.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

//var token = builder.Configuration.GetValue("BotToken", string.Empty);

builder.Services.AddDbContext<ApplicationDbContext>(configure =>
{
    configure.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies();
});
var token = "6180137021:AAGttKd_fdR_8g3x2HLerosfR4o6-qShCUw";
builder.Services.AddSingleton(t => new TelegramBotClient(token));
builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
builder.Services.AddHostedService<BackgroundTelegramBotService>();
builder.Services.AddScoped<UserService>();
builder.Services.RegisterServices();
builder.Services.AddLocalization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var supportedCultures = new[] { "tg-TJ", "en-US", "ru-RU" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[1])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.MapControllers();
app.Run();

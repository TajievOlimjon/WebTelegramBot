using Telegram.Bot;
using Telegram.Bot.Polling;

namespace WebApiTelegramBot.BackgroundServices
{
    public class BackgroundTelegramBotService : BackgroundService
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly TelegramBotClient _client;
        private readonly IUpdateHandler _updateHandler;
        public BackgroundTelegramBotService(ILogger<BackgroundService> logger, TelegramBotClient client, IUpdateHandler updateHandler)
        {
            _logger = logger;
            _client = client;
            _updateHandler = updateHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bot = await _client.GetMeAsync(stoppingToken);

            _logger.LogInformation($"{bot.Username}  bot is successfully started!");
            var receiverOptions = new ReceiverOptions()
            {
                ThrowPendingUpdates = true,
                AllowedUpdates = { }
            };
            _client.StartReceiving(
                _updateHandler.HandleUpdateAsync,
                _updateHandler.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken);
        }
    }
}

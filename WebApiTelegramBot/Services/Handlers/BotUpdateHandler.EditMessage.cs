using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApiTelegramBot.Services
{
    public partial class BotUpdateHandler
    {
        private async Task HandleEditMessageAsync(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken)
        {
            var from = message.From;

            ArgumentNullException.ThrowIfNull(message);

            _logger.LogInformation("Received edit message from {from.FirstName} , {message.Text}", from.FirstName, message.Text);
        }
    }
}

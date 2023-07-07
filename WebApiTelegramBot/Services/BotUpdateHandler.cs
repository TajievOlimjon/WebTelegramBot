using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WebApiTelegramBot.Data;
using WebApiTelegramBot.Resources;
using WebApiTelegramBot.Services.EntitiesServices;

namespace WebApiTelegramBot.Services
{
    public partial class BotUpdateHandler : IUpdateHandler
    {
        private readonly ILogger<BotUpdateHandler> _logger;
        private readonly IServiceProvider _scopeFactory;
        private IStringLocalizer _localizer;
        private UserService _userService;
        //***
        private ProgressBookService _progressBookService;
        private StudentService _studentService;
        //***
        public BotUpdateHandler(ILogger<BotUpdateHandler> logger, IServiceProvider scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Error occurred with telegram bot => {exception.Message}", exception.Message);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                _userService = scope.ServiceProvider.GetRequiredService<UserService>();
                _studentService = scope.ServiceProvider.GetRequiredService<StudentService>();
                _progressBookService = scope.ServiceProvider.GetRequiredService<ProgressBookService>();

                var culture = await GetCultureForUser(botClient,update,cancellationToken);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                _localizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<BotLocalizer>>();

                /*var chatId = update.Message.Chat.Id;
                var chatMember = await botClient.GetChatMemberAsync(chatId, update.Message.Chat.Id);*/

                //botClient.DeclineChatJoinRequest(update.Message.Chat.Id, 2544684, cancellationToken);
                // update.ChatMember.NewChatMember.Status==ChatMemberStatus.Left
                //update.ChatJoinRequest.From;
                // var res = await botClient.RestrictChatMemberAsync(update.Message.Chat.Id, 34568456845,null,null, DateTime.UtcNow.AddDays(7), cancellationToken);
               /* botClient.GetChatMemberAsync();
                botClient.LogOutAsync();
                botClient.SetChatPermissionsAsync();
                botClient.UnbanChatMemberAsync();
                botClient.UnbanChatSenderChatAsync();
                botClient.BanChatMemberAsync();
                botClient.BanChatSenderChatAsync();*/
               



                var handler = update.Type switch
                 {
                     UpdateType.Message => HandleMessageAsync(botClient, update.Message,culture, cancellationToken),
                     UpdateType.EditedMessage => HandleEditMessageAsync(botClient, update.EditedMessage, cancellationToken),
                     UpdateType.ChannelPost => HandleChannelPostAsync(botClient, update.ChannelPost, cancellationToken),
                     UpdateType.CallbackQuery => HandleCallbackQueryLanguageAsync(botClient,update, update.CallbackQuery, culture, cancellationToken),
                     _ => HandleUnknownUpdateAsync(botClient, update, cancellationToken)
                 };
                 try
                 {
                     await handler;
                 }
                 catch (Exception ex)
                 {
                     await HandlePollingErrorAsync(botClient, ex, cancellationToken);
                 }

            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
            }

        }

        public async Task<CultureInfo> GetCultureForUser(ITelegramBotClient botClient, Update update,CancellationToken cancellationToken)
        {
            try
            {
                var from = update.Type switch
                {
                    UpdateType.Message => update?.Message?.From,
                    UpdateType.EditedMessage => update?.EditedMessage?.From,
                    UpdateType.CallbackQuery => update?.CallbackQuery?.From,
                    UpdateType.InlineQuery => update?.InlineQuery?.From,
                    //UpdateType.ChannelPost => update.ChannelPost.From,
                    _ => update?.Message?.From
                };

                var user = await _userService.GetUserAsync(from?.Id);
                if (user == null)
                {
                    var result = await _userService.AddUserAcync(new Entities.ApplicationUser
                    {
                        UserId = from.Id,
                        FirstName = from.FirstName,
                        LastName = from.LastName,
                        UserName = from.Username,
                        ChatId = update.Message.Chat.Id,
                        LanguageCode = from.LanguageCode,
                        IsBot = from.IsBot,
                        JoinedAt = DateTimeOffset.UtcNow,
                        LastInteractionAt = DateTimeOffset.UtcNow
                    });
                    if (result.isSuccess)
                    {
                        _logger.LogInformation("New user added !");
                    }
                }
                var languageCode = await _userService.GetLanguageCodeAcync(from.Id);

                return new CultureInfo(languageCode ?? "ru-Ru");
            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
                return new CultureInfo("ru-Ru");
            }

        }

        private Task HandleChannelPostAsync(ITelegramBotClient botClient, Message? channelPost, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Chanel name {channelPost.SenderChat.Title} \n send message from {channelPost.Chat.FirstName} ,\n message : {channelPost.Text}", channelPost.SenderChat.Title, channelPost.Chat.FirstName, channelPost.Text);

            return Task.CompletedTask;
        }

        private Task HandleUnknownUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update type {update.Type} received", update.Type);

            return Task.CompletedTask;
        }
    }
}


using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Globalization;
using WebApiTelegramBot.Services.EntitiesServices;

namespace WebApiTelegramBot.Services
{
    public partial class BotUpdateHandler
    {
        private async Task HandleMessageAsync(ITelegramBotClient botClient, Message? message, CultureInfo culture, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(message);

            var messageType = message.Type switch
            {
                MessageType.Text => HandleTextMessageAsync(botClient, message, culture, cancellationToken),
                MessageType.Poll => HandlePollMessageAsync(botClient, message, cancellationToken),
                _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
            };
            try
            {
                await messageType;
            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
            }
        }
        private Task HandleUnknownMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received message type {message.Type}", message.Type);

            return Task.CompletedTask;
        }
        private Task HandlePollMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task HandleTextMessageAsync(ITelegramBotClient botClient, Message? message,CultureInfo culture, CancellationToken cancellationToken)
        {
            var from = message.From;
            var fullName = string.Concat(message.From.LastName, message.From.FirstName);
            _logger.LogInformation("Received message : {message.Text}," +
                " from : {fullName} ," +
                " chatId:{message.Chat.Id} ,",
                message.Text, fullName,message.Chat.Id);

            if (message.Text == "/language")
            {
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData(text:"Ru",callbackData: "ru"),
                            InlineKeyboardButton.WithCallbackData(text: "En",callbackData:"en"),

                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData(text:"Tj",callbackData:"tj")
                        }
                });

                if (culture.TextInfo.CultureName == "tg-TJ")
                {
                    var greet = $"👋 Ассалому алейкум, {message.From.Username}!";
                    await botClient.SendTextMessageAsync(message.From.Id, $"{greet}" + "\n" +
                        $"Шуморо боти DemoBot аз Софтклаб пешвоз мекунад!" +
                        $" DemoBot ассистенти таълимии шумо мебошад!\r\n" +
                        $" Забони худро интихоб намоед!",
                        parseMode: ParseMode.Html,
                        replyMarkup: inlineKeyboard);
                }
                else if (culture.TextInfo.CultureName == "en-US")
                {
                    var greet = $"👋 Hello, {message.From.Username}!\n";
                    await botClient.SendTextMessageAsync(message.From.Id, $"{greet}" + "\n" +
                        $"Welcome to the DemoBot of Softclub, your study assistant!\r\n" +
                        $"Select your language!",
                        parseMode: ParseMode.Html,
                        replyMarkup: inlineKeyboard);
                }
                else
                {
                    var greet = $"👋 Здравствуйте, {message.From.Username}!\n";
                    await botClient.SendTextMessageAsync(message.From.Id, $"{greet}" + "\n" +
                        $"Вас приветствует DemoBot Софтклаба, ваш помощник по учёбе!\r\n" +
                        $"Выберите ваш язык!",
                        parseMode: ParseMode.Html,
                        replyMarkup: inlineKeyboard);
                }
            }
            else if (message?.Text == "/auth")
            {
                await HadleAuthorizeUserAsync(botClient,message.From.Id,message,culture, cancellationToken);
            }
            else if (message?.Text == "/myaverage")
            {
                var user = await _userService.GetPhoneNumberUserAsync(message.From.Id);
                if (user.PhoneNumber == null)
                {
                    await HadleUnAuthorizeMessageUserAsync(botClient,message.From.Id,culture,cancellationToken);
                    return;
                }
                var student = await _studentService.GetStudentByUserIdAsync(message.From.Id);
                if (student.Data == null && student.StatusCode==404)
                {
                    await botClient.SendTextMessageAsync(
                          chatId: message.Chat.Id,
                          text: "Этот номер не зарегистрирован в CRM.SoftClub.tj.",
                          parseMode: ParseMode.Html,
                          replyToMessageId: message.MessageId,
                          cancellationToken: cancellationToken);
                }
                var progressbooks = _progressBookService.GetAllProgressbooksAsync(student.Data.StudentGroupDtos.GroupId, student.Data.StudentId);
                
                var grade = progressbooks.Sum(x => x.Grade);
                var IsAbsent = progressbooks.Count(x => x.IsAttended == false);
                var FullName = string.Concat(student.Data.LastName + " " + student.Data.FirstName);
                var groupName = student.Data.StudentGroupDtos.GroupName;

                var textMessage =$"Ваше ФИО: {fullName}" + "\n" + "\n" +
                        $"Ваша группа:  {groupName}" + "\n" + "\n" +
                        $"Количество отсутствие:  {IsAbsent}" + "\n" + "\n" +
                        $"Ваш бал:  {grade}";
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                      {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl(text:"Подробнее в сайте crm.softclub.tj ",url:"https://crm.softclub.tj/")
                        }
                    });
                await botClient.SendTextMessageAsync(
                          chatId: message.Chat.Id,
                          text: textMessage,
                          parseMode: ParseMode.Html,
                          replyMarkup:inlineKeyboard,
                          cancellationToken: cancellationToken);
            }
           /* else if (message?.Text == "/start")
            {
            }*/
            else
            {
                await SendMessageAsync(botClient, message, cancellationToken);
            }
        }
        private async Task SendMessageAsync(ITelegramBotClient botClient, Message? message,CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: _localizer["NotCommandDescription"],
                   parseMode: ParseMode.Html,
                   replyToMessageId: message.MessageId,
                   cancellationToken:cancellationToken);
        }
        private async Task HadleUnAuthorizeMessageUserAsync(ITelegramBotClient botClient, long fromId,CultureInfo culture, CancellationToken cancellationToken)
        {
           /* string text;
            if (culture.Parent.Name == "ru")
            {
                text = "Отменить";
            }
            else if (culture.Parent.Name == "en")
            {
                text = "Canceled";
            }
            else
            {
                text = "Катъ кардан.";
            }
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {

                new[]
                {
                     InlineKeyboardButton.WithCallbackData(text:text,callbackData:"/auth")
                }
            });*/
            await botClient.SendTextMessageAsync(
                chatId: fromId,
                text: "Вы не авторизованы и вам это команда не доступна !",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
                );
        }
        private async Task HadleAuthorizeUserAsync(ITelegramBotClient botClient,long fromId, Message message, CultureInfo culture, CancellationToken cancellationToken)
        {
            string text;
            if (culture.Parent.Name == "ru")
            {
                text = "Отменить";
            }
            else if (culture.Parent.Name == "en")
            {
                text = "Canceled";
            }
            else
            {
                text = "Катъ кардан.";
            }
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {

                new[]
                {
                     InlineKeyboardButton.WithCallbackData(text:text,callbackData:"/canceled")
                }
            });
            await botClient.SendTextMessageAsync(
                chatId:fromId,
                text: _localizer["PhoneNumber"],
                parseMode:ParseMode.Html,
                replyMarkup:inlineKeyboard,
                cancellationToken:cancellationToken
                );
        }
        private async Task HandleCallbackQueryLanguageAsync(ITelegramBotClient botClient, Update update, CallbackQuery? callbackQuery,CultureInfo culture, CancellationToken cancellationToken)
        {
            if(callbackQuery.Data == "/canceled")
            {
                string text;
                if (culture.Parent.Name == "ru")
                {
                    text = "Операция успешно отменена. 💚";
                }
                else if (culture.Parent.Name == "en")
                {
                    text = "Operation canceled successfully 💚";
                }
                else
                {
                    text = "Амалиёт бо муваффақият бекор карда шуд. 💚";
                }
                await botClient.SendTextMessageAsync(
                      chatId: callbackQuery.From.Id,
                      text: text,
                      parseMode: ParseMode.Html,
                      cancellationToken: cancellationToken
                     );
                /*await botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: text,
                    cancellationToken: cancellationToken);*/
            }
            else if (callbackQuery.Data == "/auth")
            {
                await HadleAuthorizeUserAsync(botClient,callbackQuery.From.Id, update.Message, culture, cancellationToken);
            }
            else
            {
                if (callbackQuery.Data.Contains("en", StringComparison.CurrentCultureIgnoreCase))
                {
                    await _userService.UpdateLanguageCodeAcync(callbackQuery.From.Id, "en-US");
                     culture = await GetCultureForUser(botClient, update, cancellationToken);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;

                    await botClient.SendTextMessageAsync(
                         chatId: callbackQuery.From.Id,
                        "Your language has been changed to «english»",
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                    /*await botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        text: "Your language has been changed to «english»",
                        cancellationToken: cancellationToken);*/

                    var user = await _userService.GetPhoneNumberUserAsync(callbackQuery.From.Id);
                    if (user.PhoneNumber == null)
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
                       {

                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData(text:"Pass authorization",callbackData:"/auth")
                        }
                        });

                        await botClient.SendTextMessageAsync(
                         chatId: callbackQuery.From.Id,
                         text: _localizer["RecommendationInAuthorization"],
                        parseMode: ParseMode.Html,
                        replyMarkup:inlineKeyboard,
                        cancellationToken: cancellationToken);
                    }
                }
                else if (callbackQuery.Data.Contains("ru", StringComparison.CurrentCultureIgnoreCase))
                {
                    await _userService.UpdateLanguageCodeAcync(callbackQuery.From.Id, "ru-RU");
                     culture = await GetCultureForUser(botClient, update, cancellationToken);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;

                    await botClient.SendTextMessageAsync(
                         chatId: callbackQuery.From.Id,
                        "🇷🇺 Ваш язык изменен на «русский»",
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                   /* await botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        text: "🇷🇺 Ваш язык изменен на «русский»",
                        cancellationToken: cancellationToken);*/

                    var user = await _userService.GetPhoneNumberUserAsync(callbackQuery.From.Id);
                    if (user.PhoneNumber == null)
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
                        {

                         new[]
                         {
                             InlineKeyboardButton.WithCallbackData(text:"Пройти авторизацию",callbackData:"/auth")
                         }
                       });
                        await botClient.SendTextMessageAsync(
                         chatId: callbackQuery.From.Id,
                         text: _localizer["RecommendationInAuthorization"],
                        parseMode: ParseMode.Html,
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    await _userService.UpdateLanguageCodeAcync(callbackQuery.From.Id, "tg-TJ");
                     culture = await GetCultureForUser(botClient, update, cancellationToken);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;

                    await botClient.SendTextMessageAsync(
                        chatId: callbackQuery.From.Id,
                       "🇹🇯 Забони шумо ба «тоҷики» иваз шуд.",
                       parseMode: ParseMode.Html,
                       cancellationToken: cancellationToken);
                   /* await botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        text: "🇹🇯 Забони шумо ба «тоҷики» иваз шуд.",
                        cancellationToken: cancellationToken);*/

                    var user = await _userService.GetPhoneNumberUserAsync(callbackQuery.From.Id);
                    if (user.PhoneNumber == null)
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
                        {

                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData(text:"Гузаштани авторизасия",callbackData:"/auth")
                        }
                    });

                        await botClient.SendTextMessageAsync(
                        chatId: callbackQuery.From.Id,
                        text: _localizer["RecommendationInAuthorization"],
                        parseMode: ParseMode.Html,
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
                    }
                }

            }
        }
    }
}

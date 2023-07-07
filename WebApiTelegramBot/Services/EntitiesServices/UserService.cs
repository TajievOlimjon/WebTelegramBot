using Microsoft.EntityFrameworkCore;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebApiTelegramBot.Data;
using WebApiTelegramBot.Entities;

namespace WebApiTelegramBot.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly TelegramBotClient botClient;
        public UserService(ApplicationDbContext dbContext, TelegramBotClient botClient)
        {
            _dbContext = dbContext;
            this.botClient = botClient;
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }
        public async Task<ApplicationUser?> GetPhoneNumberUserAsync(long? userId)
        {
            if (userId == 0)
            {
                return null;
            }

            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<ApplicationUser?> GetUserAsync(long? userId)
        {
            if (userId == 0)
            {
                return null;
            }

            return await _dbContext.Users.FirstOrDefaultAsync(u=>u.UserId==userId);
        }
        public async Task<string?> GetLanguageCodeAcync(long userId)
        {
            var user = await GetUserAsync(userId);

            return user?.LanguageCode;
        }
        public async Task<(bool isSuccess,string errorMessage)> UpdateLanguageCodeAcync(long userId,string languageCode)
        {
            if (languageCode == string.Empty)
            {
                return (false, "Language is null !");
            }

            var user = await GetUserAsync(userId);
            if (user is null)
            {
                return (false, "User not found !");
            }

            user.LanguageCode = languageCode;
            user.LastInteractionAt = DateTimeOffset.UtcNow;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return new(true,"Success !");
        }
        public async Task<(bool isSuccess, string errorMessage)> AddUserAcync(ApplicationUser model)
        {
            try
            {
                await _dbContext.Users.AddAsync(model);
                var result = await _dbContext.SaveChangesAsync();

                return (true, "Success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool isSuccess, string errorMessage)> UpdatePhoneNumberUserAcync(ApplicationUser model)
        {
            try
            {
                var user = await GetUserAsync(model.UserId);
                if (user is null)
                {
                    return (false, "User not found !");
                }

                user.PhoneNumber = model.PhoneNumber;
                user.LastInteractionAt = DateTimeOffset.UtcNow;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return new(true, "Success !");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<Response<string>> BlockedUser(long userId)
        {
            var user = await GetUserAsync(userId);
            if (user == null) return new Response<string>(System.Net.HttpStatusCode.NotFound, "User not found !");
            await botClient.RestrictChatMemberAsync(user.ChatId, user.UserId, new ChatPermissions
            {
                CanSendMessages = false,
                CanSendPolls = false,
                CanSendOtherMessages = false,
                CanAddWebPagePreviews = false,
                CanChangeInfo = false,
                CanInviteUsers = false,
                CanPinMessages = false,
                CanSendDocuments = false,
                CanSendAudios = false,
                CanManageTopics = false,
                CanSendPhotos = false,
                CanSendVideoNotes = false,
                CanSendVideos = false,
                CanSendVoiceNotes = false
            }, null, DateTime.UtcNow.AddDays(7), new CancellationToken());

            return new Response<string>(System.Net.HttpStatusCode.OK, "User Successfully blocked !");
        }
        public async Task<Response<string>> UnBlockedUser(long userId)
        {
            var user = await GetUserAsync(userId);
            if (user == null) return new Response<string>(System.Net.HttpStatusCode.NotFound, "User not found !");
            await botClient.RestrictChatMemberAsync(user.ChatId, user.UserId, new ChatPermissions
            {
                CanSendMessages = true,
                CanSendPolls = true,
                CanSendOtherMessages = true,
                CanAddWebPagePreviews = true,
                CanChangeInfo = true,
                CanInviteUsers = true,
                CanPinMessages = true,
                CanSendDocuments = true,
                CanSendAudios = true,
                CanManageTopics = true,
                CanSendPhotos = true,
                CanSendVideoNotes = true,
                CanSendVideos = true,
                CanSendVoiceNotes = true
            }, null, DateTime.UtcNow.AddDays(7), new CancellationToken());

            return new Response<string>(System.Net.HttpStatusCode.OK, "User Successfully Unblocked !");
        }
    }
}

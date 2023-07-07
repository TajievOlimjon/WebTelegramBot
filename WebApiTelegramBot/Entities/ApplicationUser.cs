using System.ComponentModel.DataAnnotations;

namespace WebApiTelegramBot.Entities
{
    public class ApplicationUser
    {
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public bool IsBot { get; set; }=false;
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? UserName { get; set; } = null;
        public string? LanguageCode { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public DateTimeOffset JoinedAt { get; set; }
        public DateTimeOffset LastInteractionAt { get;set; }
    }
}



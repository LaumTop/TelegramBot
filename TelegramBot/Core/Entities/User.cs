using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.Entities
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public required long telegramId { get; set; }
        public required string username { get; set; }
        public required string firstName { get; set; }
        public required DateTime registeredAt { get; set; }

    }
}

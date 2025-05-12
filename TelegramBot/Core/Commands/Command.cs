using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Attributes;
using TelegramBot.Core.Interfaces;
using System.Reflection;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Core.Commands
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }

        public virtual IEnumerable<string> Aliases => Enumerable.Empty<string>();

        public async Task<bool> CanExecute(Update update, ITelegramBotClient bot)
        {
            var message = update.Message;
            if (message?.Text == null || message.From == null)
                return false;

            var cmd = message.Text.Trim();
            if (!cmd.Equals(Name, StringComparison.OrdinalIgnoreCase) &&
                !Aliases.Any(a => cmd.Equals(a, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var chatType = GetType().GetCustomAttribute<ChatTypeAttribute>();
            if (chatType != null)
            {
                if (chatType.ChatType == ChatType.Private && message.Chat.Type != ChatType.Private ||
                    chatType.ChatType == ChatType.Group &&
                    (message.Chat.Type != ChatType.Group && message.Chat.Type != ChatType.Supergroup) ||
                    chatType.ChatType == ChatType.Supergroup && message.Chat.Type != ChatType.Supergroup)
                {
                    return false;
                }
            }

            var role = GetType().GetCustomAttribute<RoleAttribute>();
            if (role != null)
            {
                var member = await bot.GetChatMember(message.Chat.Id, message.From.Id);

                int userRank = member.Status switch
                {
                    ChatMemberStatus.Creator => 3,
                    ChatMemberStatus.Administrator => 2,
                    ChatMemberStatus.Member => 1,
                    _ => 0
                };

                int requiredRank = role.Role switch
                {
                    ChatMemberStatus.Creator => 3,
                    ChatMemberStatus.Administrator => 2,
                    ChatMemberStatus.Member => 1,
                    _ => 0
                };

                if (userRank < requiredRank)
                    return false;
            }

            return true;
        }

        public abstract Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken);
    }
}

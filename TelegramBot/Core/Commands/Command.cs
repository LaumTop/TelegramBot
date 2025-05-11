using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Attributes;
using TelegramBot.Core.Interfaces;
using System.Reflection;

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
            var cmd = message.Text.Trim().ToLower();
            if (!(cmd == Name.ToLower() || Aliases.Any(a => a.ToLower() == cmd)))
            {
                return false;
            }

                var chatType = GetType().GetCustomAttribute<ChatTypeAttribute>();
            if (chatType != null)
            {
                switch (chatType.ChatType)
                {
                    case TelegramBot.Core.Enums.ChatType.Private:
                        if (message.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private)
                            return false;
                        break;
                    case TelegramBot.Core.Enums.ChatType.Group:
                        if (message.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Group &&
                            message.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Supergroup)
                            return false;
                        break;
                    case TelegramBot.Core.Enums.ChatType.SuperGroup:
                        if (message.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Supergroup)
                            return false;
                        break;
                    case TelegramBot.Core.Enums.ChatType.All:
                        break;
                }
            }

            var role = GetType().GetCustomAttribute<RoleAttribute>();
            if (role != null)
            {
                var member = await bot.GetChatMember(message.Chat.Id, message.From.Id);
                if (member.Status < role.Role)
                    return false;
            }
            return true;
        }

        public abstract Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken);
    }
}

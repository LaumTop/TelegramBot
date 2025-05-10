using TelegramBot.Core.Utils;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Core.Interfaces;
using TelegramBot.Core.Attributes;
using TelegramBot.Core.Events;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Core.Services
{
    class BotService : IBotService
    {
        private readonly EventDispatcher _dispatcher;
        public BotService(EventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        public static string? token { private set; get; }
        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token, ICollection<ICommand> commands)
        {
            if (update.MyChatMember != null &&
                (update.MyChatMember.Chat.Type == ChatType.Group || update.MyChatMember.Chat.Type == ChatType.Supergroup) &&
                update.MyChatMember.NewChatMember.User.IsBot &&
                update.MyChatMember.NewChatMember.Status == ChatMemberStatus.Member)
            {
                await _dispatcher.NotifyBotAddedToGroup(update.MyChatMember.Chat, bot);
            }
            if (update.Message is not { Text: { } messageText } message)
                return;
            foreach (var command in commands)
            {
                if (await command.CanExecute(update, bot))
                {
                    await command.ExecuteAsync(bot, message, token);
                    break;
                }
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
        {
            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Red, $"[ERROR] Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}

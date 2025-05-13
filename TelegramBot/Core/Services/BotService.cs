using TelegramBot.Core.Utils;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Core.Interfaces;
using TelegramBot.Core.Attributes;
using TelegramBot.Core.Events;
using Telegram.Bot.Types.Enums;
using Serilog;
using TelegramBot.Core.Commands;

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
                Log.Information("Bot added to group: {GroupName}", update.MyChatMember.Chat.Title);
                return;
            }

            if (update.Message != null)
            {
                var message = update.Message;

                if(message.Chat.Type == ChatType.Private)
                {
                    if(message.Contact != null) await _dispatcher.NotifyPrivChatContactReceived(message, bot);
                    else if(update.Type == UpdateType.EditedMessage) await _dispatcher.NotifyPrivChatEditedMessageReceived(message, bot);
                    else if (message.Location != null) await _dispatcher.NotifyPrivChatLocationReceived(message, bot);
                    else if (message.ReplyToMessage != null) await _dispatcher.NotifyPrivChatMessageReply(message.Chat, message.ReplyToMessage, bot);
                }

                if ((message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup) && message.From != null)
                {
                    await _dispatcher.NotifyMessageSentInGroup(message.Chat, message, bot);
                } else if(message.Chat.Type == ChatType.Private && message.From != null)
                {
                    await _dispatcher.NotifyPrivChatMessageSend(message.Chat, message, bot);
                }

                if (message.NewChatMembers != null)
                {
                    foreach (var newUser in message.NewChatMembers)
                    {
                        await _dispatcher.NotifyUserJoinedInGroup(message.Chat, newUser, bot);
                    }
                }

                if (message.LeftChatMember != null)
                {
                    await _dispatcher.NotifyUserLeftGroup(message.Chat, message.LeftChatMember, bot);
                }

                foreach (var command in commands)
                {
                    if (await command.CanExecute(update, bot))
                    {
                        await command.ExecuteAsync(bot, message, token);
                        if(message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup) await _dispatcher.NotifyCommandExecuted(message.Chat, message.From!, command as Command, bot);
                        else if(message.Chat.Type == ChatType.Private) await _dispatcher.NotifyPrivChatCommandExecuted(message.Chat, message.From!, command as Command, bot);
                        break;
                    }
                }
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
        {
            Log.Error(exception, "Error occurred: {Message}", exception.Message);
            return Task.CompletedTask;
        }
    }
}


using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Commands;

namespace TelegramBot.Core.Events
{
    //Class for handling private chat events
    public abstract class PrivateListener
    {
        public Task ExecuteOnMessageSend(Chat chat, Message message, ITelegramBotClient bot) =>
            HandleGroupEvent(chat, bot, (grp, b) => OnMessageSent(grp, message, b));
        public Task ExecuteOnCommandExecute(Chat chat, User user, Command command, ITelegramBotClient bot) =>
            HandleGroupEvent(chat, bot, (grp, b) => OnCommandExecute(grp, user, command, b));
        public Task ExecuteOnEditedMessageReceived(Message editedMessage, ITelegramBotClient bot) =>
            HandleGroupEvent(editedMessage.Chat, bot, (grp, b) => OnEditedMessageReceived(editedMessage, b));
        public Task ExecuteOnMessageReply(Chat chat, Message replyMessage, ITelegramBotClient bot) =>
            HandleGroupEvent(chat, bot, (grp, b) => OnMessageReply(grp, replyMessage, b));
        public Task ExecuteOnContactReceived(Message message, ITelegramBotClient bot) =>
            HandleGroupEvent(message.Chat, bot, (grp, b) => OnContactReceived(message, b));
        public Task ExecuteOnLocationReceived(Message message, ITelegramBotClient bot) =>
            HandleGroupEvent(message.Chat, bot, (grp, b) => OnLocationReceived(message, b));

        protected virtual Task OnMessageSent(Chat chat, Message message, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnCommandExecute(Chat chat, User user, Command command, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnEditedMessageReceived(Message editedMessage, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnMessageReply(Chat chat, Message replyMessage, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnContactReceived(Message message, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnLocationReceived(Message message, ITelegramBotClient bot) => Task.CompletedTask;

        //Maybe later replace in static class
        private static Task HandleGroupEvent(Chat group, ITelegramBotClient bot, Func<Chat, ITelegramBotClient, Task> action)
        {
            ValidateChat(group);
            return action(group, bot);
        }

        private static void ValidateChat(Chat Chat)
        {
            if (Chat == null) throw new ArgumentNullException(nameof(Chat));
            if (Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private)
            {
                return;
            }
        }
    }
}

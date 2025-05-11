using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Commands;

namespace TelegramBot.Core.Events
{
    public abstract class GroupListener
    {
        public Task ExecuteOnBotAddedToGroup(Chat group, ITelegramBotClient bot) =>
            HandleGroupEvent(group, bot, OnBotAddedToGroup);

        public Task ExecuteUserJoinedInGroup(Chat group, User user, ITelegramBotClient bot)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return HandleGroupEvent(group, bot, (grp, b) => OnUserJoinedInGroup(grp, user, b));
        }

        public Task ExecuteMessageSentInGroup(Chat group, Message message, ITelegramBotClient bot)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            return HandleGroupEvent(group, bot, (grp, b) => OnMessageSentInGroup(grp, message, b));
        }

        public Task ExecuteUserLeftGroup(Chat group, User user, ITelegramBotClient bot)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return HandleGroupEvent(group, bot, (grp, b) => OnUserLeftGroup(grp, user, b));
        }

        public Task ExecuteCommandExecuted(Chat group, User user, Command command, ITelegramBotClient bot)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (command == null) throw new ArgumentNullException(nameof(command));
            return HandleGroupEvent(group, bot, (grp, b) => OnCommandExecute(grp, user, command, b));
        }

        protected virtual Task OnBotAddedToGroup(Chat group, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnUserJoinedInGroup(Chat group, User user, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnMessageSentInGroup(Chat group, Message message, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnUserLeftGroup(Chat group, User user, ITelegramBotClient bot) => Task.CompletedTask;
        protected virtual Task OnCommandExecute(Chat group, User user, Command command, ITelegramBotClient bot) => Task.CompletedTask;

        private void ValidateGroup(Chat group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            if (group.Type != Telegram.Bot.Types.Enums.ChatType.Group &&
                group.Type != Telegram.Bot.Types.Enums.ChatType.Supergroup)
            {
                return;
            }
        }

        private Task HandleGroupEvent(Chat group, ITelegramBotClient bot, Func<Chat, ITelegramBotClient, Task> action)
        {
            ValidateGroup(group);
            return action(group, bot);
        }
    }
}
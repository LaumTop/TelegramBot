using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Commands;

namespace TelegramBot.Core.Events
{
    public class EventDispatcher
    {
        private readonly List<GroupListener> _listeners = new();

        public void RegisterListener(GroupListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(GroupListener listener)
        {
            _listeners.Remove(listener);
        }

        private async Task NotifyListeners(Func<GroupListener, Task> notifyAction)
        {
            foreach (var listener in _listeners)
            {
                await notifyAction(listener);
            }
        }

        public Task NotifyBotAddedToGroup(Chat group, ITelegramBotClient bot) =>
            NotifyListeners(l => l.ExecuteOnBotAddedToGroup(group, bot));

        public Task NotifyUserJoinedInGroup(Chat group, User user, ITelegramBotClient bot) =>
            NotifyListeners(l => l.ExecuteUserJoinedInGroup(group, user, bot));

        public Task NotifyUserLeftGroup(Chat group, User user, ITelegramBotClient bot) =>
            NotifyListeners(l => l.ExecuteUserLeftGroup(group, user, bot));

        public Task NotifyMessageSentInGroup(Chat group, Message message, ITelegramBotClient bot) =>
            NotifyListeners(l => l.ExecuteMessageSentInGroup(group, message, bot));

        public Task NotifyCommandExecuted(Chat group, User user, Command command, ITelegramBotClient bot) =>
            NotifyListeners(l => l.ExecuteCommandExecuted(group, user, command, bot));
    }
}
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Core.Events
{
    public class EventDispatcher
    {
        private readonly List<Listener> _listeners = new();
        public void RegisterListener(Listener listener)
        {
            _listeners.Add(listener);
        }
        public void UnregisterListener(Listener listener)
        {
            _listeners.Remove(listener);
        }
        public async Task NotifyBotAddedToGroup(Chat group, ITelegramBotClient bot)
        {
            foreach (var listener in _listeners)
            {
                await listener.ExecuteOnBotAddedToGroup(group, bot);
            }
        }
        public async Task NotifyUserJoinedInGroup(Chat group, User user, ITelegramBotClient bot)
        {
            foreach (var listener in _listeners)
            {
                await listener.ExecuteUserJoinedInGroup(group, user, bot);
            }
        }
    }
}

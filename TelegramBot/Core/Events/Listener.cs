using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Core.Events
{
    public abstract class Listener
    {
        public async Task ExecuteOnBotAddedToGroup(Chat group, ITelegramBotClient bot)
        {
            if(group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            if(group.Type != Telegram.Bot.Types.Enums.ChatType.Group && group.Type != Telegram.Bot.Types.Enums.ChatType.Supergroup)
            {
                throw new ArgumentException("Invalid chat type. Expected Group or Supergroup.");
            }
            await OnBotAddedToGroup(group, bot);
        }
        public async Task ExecuteUserJoinedInGroup(Chat group, User user, ITelegramBotClient bot)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (group.Type != Telegram.Bot.Types.Enums.ChatType.Group && group.Type != Telegram.Bot.Types.Enums.ChatType.Supergroup)
            {
                throw new ArgumentException("Invalid chat type. Expected Group or Supergroup.");
            }
            await UserJoinedInGroup(group, user, bot);
        }
        protected virtual async Task OnBotAddedToGroup(Chat group, ITelegramBotClient bot)
        {
            // Default implementation (if any)
        }
        protected virtual async Task UserJoinedInGroup(Chat group, User user, ITelegramBotClient bot)
        {
            // Default implementation (if any)
        }
    }
}

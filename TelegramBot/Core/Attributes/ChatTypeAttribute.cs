using System;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Core.Attributes
{
    //Attribute to specify the type of chat the command can be executed in
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ChatTypeAttribute : Attribute
    {
        public ChatType? ChatType { get; }

        public ChatTypeAttribute(ChatType chatType)
        {
            ChatType = chatType;
        }
        public ChatTypeAttribute()
        {
            ChatType = null; //Chat type is not specified, will work all chats, if attribute is not set, all chats will  work
        }
    }
}

using System;
using TelegramBot.Core.Enums;

namespace TelegramBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ChatTypeAttribute : Attribute
    {
        public ChatType ChatType { get; }

        public ChatTypeAttribute(ChatType chatType)
        {
            ChatType = chatType;
        }
        public ChatTypeAttribute()
        {
            ChatType = ChatType.All;
        }
    }
}

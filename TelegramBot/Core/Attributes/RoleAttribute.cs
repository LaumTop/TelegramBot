
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RoleAttribute : Attribute
    {
        public ChatMemberStatus Role { get; }
        public RoleAttribute(ChatMemberStatus role)
        {
            Role = role;
        }
        public RoleAttribute()
        {
            Role = ChatMemberStatus.Member;
        }
    }
}

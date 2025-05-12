
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Core.Attributes
{
    // Attribute to specify the role of the user in the chat
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

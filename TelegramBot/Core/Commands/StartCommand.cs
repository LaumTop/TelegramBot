using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Attributes;
using Telegram.Bot.Types.Enums;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Commands
{
    [Role(Telegram.Bot.Types.Enums.ChatMemberStatus.Member)]
    [ChatType(new ChatType())]
    class StartCommand : Command
    {
        private readonly IUserService _users;
        public StartCommand(IUserService users)
        {
            _users = users;
        }
        public override string Name => "/start";
        public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var text = "Welcome to the bot!";
            await bot.SendMessage(chatId, text, cancellationToken: cancellationToken);
        }
    }
}

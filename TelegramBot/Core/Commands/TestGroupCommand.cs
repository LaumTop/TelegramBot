using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Attributes;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Core.Commands
{
    [Role(Telegram.Bot.Types.Enums.ChatMemberStatus.Member)]
    [ChatType(ChatType.Group)]
    class TestGroupCommand : Command
    {
        public override string Name => "/testgroup";
        public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var text = "This is a test group command!";
            await bot.SendMessage(chatId, text, cancellationToken: cancellationToken);
        }
    }
}

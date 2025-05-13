using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Attributes;
using Telegram.Bot.Types.Enums;
using TelegramBot.Core.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Core.Commands
{
    //Example of command
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
            if (message?.From == null)
                return; // Probably will never happen, but just in case
            var chatId = message.Chat.Id;
            if (await _users.IsUserExistsAsync(message.From.Id))
            {
                //TODO: Send information and maybe show menu, with functionality
                return;
            }
            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                        new KeyboardButton("Share Contact") { RequestContact = true }
                    })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
            await bot.SendMessage(chatId, "Welcome to the bot\nYou need to register to start using it.", replyMarkup: keyboard, cancellationToken: cancellationToken);
        }
    }
}

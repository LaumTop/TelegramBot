using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Core.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        IEnumerable<string> Aliases { get; }
        Task<bool> CanExecute(Update update, ITelegramBotClient bot);

        Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken);
    }
}
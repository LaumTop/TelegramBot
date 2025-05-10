using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Core.Interfaces
{
    public interface IBotService
    {
        Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token, ICollection<ICommand> commands);
        Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token);
    }
}

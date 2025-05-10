using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Core.Attributes;
using TelegramBot.Core.Contexts;
using TelegramBot.Core.Entities;
using TelegramBot.Core.Events;
using TelegramBot.Core.Interfaces;
using TelegramBot.Core.Services;
using TelegramBot.Core.Utils;

namespace TelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var services = new ServiceCollection();

            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Cyan, "[INFO] Starting bot...");
            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Yellow, "[INFO] Initializing token...");

            string? token = config["TOKEN"];
            if (string.IsNullOrEmpty(token))
            {
                ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Red, "[ERROR] Token is not set. Please set the TOKEN environment variable.");
                return;
            }

            try
            {
                services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token));
            }
            catch (Exception ex)
            {
                ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Red, $"[ERROR] Initialization failed: {ex.Message}");
                throw;
            }

            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Cyan, "[INFO] Loading services");

            services.AddSingleton<IBotService, BotService>();
            services.AddScoped<IUserService, UserService>();
            services.AddDbContext<UserContext>();
            services.AddSingleton<EventDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<EventDispatcher>();
            bool hasErrors;

            var _commands = Loader.LoadCommands(services, out hasErrors);
            services.AddSingleton<ICollection<ICommand>>(_ => _commands);

            if (hasErrors)
                ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Yellow, "\n[WARNING] Some command(s) have issues.");
            else
                ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Green, "\n[SUCCESS] All commands loaded!");

            Loader.LoadListeners(services, dispatcher);
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            var botClient = provider.GetRequiredService<ITelegramBotClient>();
            var botService = provider.GetRequiredService<IBotService>();

            botClient.StartReceiving(
                async (client, update, token) => await botService.HandleUpdateAsync(client, update, token, _commands),
                async (client, exception, token) => await botService.HandleErrorAsync(client, exception, token),
                receiverOptions,
                cancellationToken: cts.Token
            );

            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Green, "[INFO] Bot started!");
            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Cyan, "[INFO] Press enter to exit");

            Console.ReadLine();
            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Yellow, "[INFO] Stopping bot...");
        }
    }
}
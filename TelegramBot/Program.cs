using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Serilog;
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

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting bot...");

            var services = new ServiceCollection();

            Log.Information("Initializing token...");

            string? token = config["TOKEN"];
            if (string.IsNullOrEmpty(token))
            {
                Log.Error("Token is not set. Please set the TOKEN environment variable.");
                return;
            }

            try
            {
                services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token));
                Log.Information("Token initialized successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Initialization failed: {Message}", ex.Message);
                throw;
            }

            Log.Information("Loading services");

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
            {
                Log.Warning("Some command(s) have issues.");
            }

            Loader.LoadListeners(services, dispatcher);

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            var botClient = provider.GetRequiredService<ITelegramBotClient>();
            var botService = provider.GetRequiredService<IBotService>();

            Log.Information("Starting receiving updates...");

            botClient.StartReceiving(
                async (client, update, token) => await botService.HandleUpdateAsync(client, update, token, _commands),
                async (client, exception, token) => await botService.HandleErrorAsync(client, exception, token),
                receiverOptions,
                cancellationToken: cts.Token
            );
            Log.Information("Bot started!");
            Log.Information("Press enter to exit");

            Console.ReadLine();

            Log.Information("Stopping bot...");

            Log.CloseAndFlush();
        }
    }
}
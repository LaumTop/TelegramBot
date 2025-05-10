using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Core.Attributes;
using TelegramBot.Core.Events;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Utils
{
    public static class Loader
    {
        public static List<ICommand> LoadCommands(ServiceCollection services, out bool hasErrors)
        {
            var commands = new List<ICommand>();
            var commandTypes = ReflectionUtils.GetAllAssignableTypes(typeof(ICommand));
            hasErrors = false;

            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Cyan, "[INFO] Loading commands: \n");

            var provider = services.BuildServiceProvider();
            foreach (var type in commandTypes)
            {
                if (ActivatorUtilities.CreateInstance(provider, type) is ICommand command)
                {
                    var role = command.GetType().GetCustomAttributes(typeof(RoleAttribute), true).FirstOrDefault() as RoleAttribute;
                    var chatType = command.GetType().GetCustomAttributes(typeof(ChatTypeAttribute), true).FirstOrDefault() as ChatTypeAttribute
                                   ?? new ChatTypeAttribute(TelegramBot.Core.Enums.ChatType.All);

                    if (role != null)
                    {
                        services.AddSingleton(command);
                        ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Green, $"[SUCCESS] Command {command.Name.Substring(1)} loaded!");
                        commands.Add(command);
                    }
                    else
                    {
                        ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Red, $"[ERROR] Command {command.Name.Substring(1)} is not loaded, role attribute required");
                        hasErrors = true;
                    }
                }
            }

            return commands;
        }
        public static void LoadListeners(ServiceCollection services, EventDispatcher dispatcher)
        {
            var listenerTypes = ReflectionUtils.GetAllAssignableTypes(typeof(Listener));
            var provider = services.BuildServiceProvider();

            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Cyan, "[INFO] Loading events");

            foreach (var type in listenerTypes)
            {
                if (ActivatorUtilities.CreateInstance(provider, type) is Listener listener)
                {
                    dispatcher.RegisterListener(listener);
                }
            }

            ColoredText.SetConsoleColorAndWriteLine(ConsoleColor.Green, "[SUCCESS] All events loaded!");
        }
    }
}

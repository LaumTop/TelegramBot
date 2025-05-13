using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Core.Attributes;
using TelegramBot.Core.Events;
using TelegramBot.Core.Interfaces;
using Serilog;

namespace TelegramBot.Core.Utils
{
    public static class Loader
    {
        public static List<ICommand> LoadCommands(ServiceCollection services, out bool hasErrors)
        {
            var commands = new List<ICommand>();
            var commandTypes = ReflectionUtils.GetAllAssignableTypes(typeof(ICommand));
            hasErrors = false;

            Log.Information("Loading commands:");

            var provider = services.BuildServiceProvider();
            foreach (var type in commandTypes)
            {
                if (ActivatorUtilities.CreateInstance(provider, type) is ICommand command)
                {
                    var role = command.GetType().GetCustomAttributes(typeof(RoleAttribute), true).FirstOrDefault() as RoleAttribute;
                    var chatType = command.GetType().GetCustomAttributes(typeof(ChatTypeAttribute), true).FirstOrDefault() as ChatTypeAttribute
                                   ?? new ChatTypeAttribute(new Telegram.Bot.Types.Enums.ChatType());

                    if (role != null)
                    {
                        if (commands.FirstOrDefault(commands => commands.Name == command.Name) != null)
                        {
                            Log.Error("Command {CommandName} is not loaded, name already exists. Check {CommandType}",
                                      command.Name.Substring(1), command.GetType().Name);
                            hasErrors = true;
                            continue;
                        }
                        services.AddSingleton(command);
                        Log.Information("Command {CommandName} loaded", command.Name.Substring(1));
                        commands.Add(command);
                    }
                    else
                    {
                        Log.Error("Command {CommandName} is not loaded, role attribute required", command.Name.Substring(1));
                        hasErrors = true;
                    }
                }
            }

            return commands;
        }

        public static void LoadListeners(ServiceCollection services, EventDispatcher dispatcher)
        {
            var listenerTypes = ReflectionUtils.GetAllAssignableTypes(typeof(GroupListener))
                .Union(ReflectionUtils.GetAllAssignableTypes(typeof(PrivateListener)));
            var provider = services.BuildServiceProvider();

            Log.Information("Loading events");

            foreach (var type in listenerTypes)
            {
                if (ActivatorUtilities.CreateInstance(provider, type) is GroupListener listener)
                {
                    dispatcher.RegisterListener(listener);
                    Log.Information("Gorup event listener {ListenerType} registered", listener.GetType().Name);
                } if(ActivatorUtilities.CreateInstance(provider, type) is PrivateListener privateListener)
                {
                    dispatcher.RegisterListener(privateListener);
                    Log.Information("Private event listener {ListenerType} registered", privateListener.GetType().Name);
                }
            }

            Log.Information("All events loaded!");
        }
    }
}

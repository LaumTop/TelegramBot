using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Core.Events;
using TelegramBot.Core.Commands;

public class EventDispatcher
{
    private readonly List<GroupListener> _groupListeners = new();
    private readonly List<PrivateListener> _privateListeners = new();

    public void RegisterListener(GroupListener listener) => _groupListeners.Add(listener);
    public void RegisterListener(PrivateListener listener) => _privateListeners.Add(listener);

    public void UnregisterListener(GroupListener listener) => _groupListeners.Remove(listener);
    public void UnregisterListener(PrivateListener listener) => _privateListeners.Remove(listener);

    private async Task NotifyGroupListeners(Func<GroupListener, Task> notifyAction)
    {
        foreach (var listener in _groupListeners)
        {
            await notifyAction(listener);
        }
    }
    private async Task NotifyPrivateChatListener(Func<PrivateListener, Task> notifyAction)
    {
        foreach (var listener in _privateListeners)
        {
            await notifyAction(listener);
        }
    }

    // Group chat event notifications
    public Task NotifyBotAddedToGroup(Chat group, ITelegramBotClient bot) =>
        NotifyGroupListeners(l => l.ExecuteOnBotAddedToGroup(group, bot));
    public Task NotifyUserJoinedInGroup(Chat group, User user, ITelegramBotClient bot) =>
        NotifyGroupListeners(l => l.ExecuteUserJoinedInGroup(group, user, bot));
    public Task NotifyUserLeftGroup(Chat group, User user, ITelegramBotClient bot) =>
        NotifyGroupListeners(l => l.ExecuteUserLeftGroup(group, user, bot));
    public Task NotifyMessageSentInGroup(Chat group, Message message, ITelegramBotClient bot) =>
        NotifyGroupListeners(l => l.ExecuteMessageSentInGroup(group, message, bot));
    public Task NotifyCommandExecuted(Chat group, User user, Command command, ITelegramBotClient bot) =>
        NotifyGroupListeners(l => l.ExecuteCommandExecuted(group, user, command, bot));

    // Private chat event notifications
    public Task NotifyPrivChatMessageSend(Chat chat, Message message, ITelegramBotClient bot) =>
        NotifyPrivateChatListener(l => l.ExecuteOnMessageSend(chat, message, bot));
    public Task NotifyPrivChatCommandExecuted(Chat chat, User user, Command command, ITelegramBotClient bot) =>
        NotifyPrivateChatListener(l => l.ExecuteOnCommandExecute(chat, user, command, bot));
    public Task NotifyPrivChatEditedMessageReceived(Message editedMessage, ITelegramBotClient bot) =>
        NotifyPrivateChatListener(l => l.ExecuteOnEditedMessageReceived(editedMessage, bot));
    public Task NotifyPrivChatMessageReply(Chat chat, Message replyMessage, ITelegramBotClient bot) =>
        NotifyPrivateChatListener(l => l.ExecuteOnMessageReply(chat, replyMessage, bot));
    public Task NotifyPrivChatContactReceived(Message message, ITelegramBotClient bot) =>
        NotifyPrivateChatListener(l => l.ExecuteOnContactReceived(message, bot));
    public Task NotifyPrivChatLocationReceived(Message message, ITelegramBotClient bot) =>
        NotifyPrivateChatListener(l => l.ExecuteOnLocationReceived(message, bot));
}
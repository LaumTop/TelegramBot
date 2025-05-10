using System;
using TelegramBot.Core.Entities;

namespace TelegramBot.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsUserExistsAsync(long userId);
        Task AddUserAsync(User user);
        Task RemoveUserAsync(long userId);
    }
}

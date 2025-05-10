using Microsoft.EntityFrameworkCore;
using System;
using TelegramBot.Core.Contexts;
using TelegramBot.Core.Entities;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        public UserService(UserContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            if(this.IsUserExistsAsync(user.telegramId).Result)
            {
                throw new Exception("User already exists");
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserExistsAsync(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.telegramId == userId) != null;
        }

        public async Task RemoveUserAsync(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.telegramId == userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}

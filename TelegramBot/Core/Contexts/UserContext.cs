using Microsoft.EntityFrameworkCore;
using TelegramBot.Core.Entities;

namespace TelegramBot.Core.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=databse.db");
        }
    }
}

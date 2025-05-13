using Microsoft.EntityFrameworkCore;
using TelegramBot.Core.Entities;

namespace TelegramBot.Core.Contexts
{
    public class UserContext : DbContext
    {
        // This is the database context for the User entity
        // This is just example code, you can modify it according to your needs
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=databse.db");
        }
    }
}

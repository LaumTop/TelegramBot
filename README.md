
# TelegramBot

## Description

This is a Telegram bot built with C# and .NET 8.0. The project follows a clean architecture using Commands, Events, Services, and Dependency Injection. It uses Entity Framework Core with SQLite for data storage and Serilog for logging.

### Features

- Handles bot commands (e.g. `/start`)
- Supports both group and private chats
- Role-based access using custom attributes
- Stores user data with SQLite and EF Core
- Logs activities with Serilog
- Secure configuration using .NET User Secrets

## Project Structure

```
TelegramBot/
├── Core/
│   ├── Attributes/       # Custom attributes for roles and chat types
│   ├── Commands/         # Command definitions and logic
│   ├── Events/           # Event listeners (group/private)
│   ├── Interfaces/       # Service and command interfaces
│   ├── Services/         # Bot and user business logic
│   ├── Contexts/         # EF Core DB context
│   └── Entities/         # Data models (e.g., User)
├── databse.db            # SQLite database
├── Program.cs            # Application entry point
└── TelegramBot.csproj    # Project file and dependencies
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- A Telegram Bot Token from [BotFather](https://t.me/BotFather)

### Installation

Follow these steps to get the bot up and running:

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/TelegramBot.git
   cd TelegramBot
   ```

2. **Restore and build the project**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Configure your bot token using .NET User Secrets**
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "TOKEN" "your-telegram-bot-token"
   ```

4. **Run the bot**
   ```bash
   dotnet run --project TelegramBot
   ```

## Dependencies

- [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
- [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- [Serilog](https://serilog.net/)
- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection)
- [Microsoft.Extensions.Configuration.UserSecrets](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets)

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DiscordBot
{
    class Program
    {
        public IConfigurationRoot Configuration { get; }

        public Program(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                            .AddJsonFile("config.json");
            Configuration = builder.Build();
        }

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            new Program(args).MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                services.GetRequiredService<LoggingService>();

                await client.LoginAsync(TokenType.Bot, Configuration["token"]);
                await client.StartAsync();

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                await Task.Delay(-1);
            }
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<LoggingService>()
                .AddSingleton(Configuration)
                .AddLogging(configure => configure.AddSerilog())
                .BuildServiceProvider();
        }
    }
}
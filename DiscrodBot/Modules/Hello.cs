using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Hello : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger _logger;

        public Hello(IServiceProvider services)
        {
            _logger = services.GetRequiredService<ILogger<Hello>>();

        }

        [Command("hello")]
        public async Task HelloCommand()
        {
            var sb = new StringBuilder();
            var user = Context.User;
            sb.AppendLine($"You are -> []");
            sb.AppendLine("I must now say, World!");
            await ReplyAsync(sb.ToString());
        }
    }
}

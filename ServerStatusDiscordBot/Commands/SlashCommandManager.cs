using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace ServerStatusDiscordBot.Commands {
    public class SlashCommandManager {

        private static readonly Dictionary<string, BaseSlashCommand> SlashCommands = new();

        public static void RegisterSlashCommand(DiscordSocketClient client) {
            try {
                  _ = CreateGrobalCommand<StatusSlashCommand>(client);

            } catch (HttpException exception) {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        private static async Task CreateGuildCommand<T>(DiscordSocketClient client, ulong guildId) where T : BaseSlashCommand, new() {
            var command = new T();
            SlashCommands.Add(command.Name, command);
            Console.WriteLine($"[ServerStatusDiscordBot]    Register [{command.Name}] Guild Command");
            await client.Rest.CreateGuildCommand(command.CommandBuilder().Build(), guildId);
        }

        private static async Task CreateGrobalCommand<T>(DiscordSocketClient client) where T : BaseSlashCommand, new() {
            var command = new T();
            SlashCommands.Add(command.Name, command);
            Console.WriteLine($"[ServerStatusDiscordBot]    Register [{command.Name}] Global Command");
            await client.Rest.CreateGlobalCommand(command.CommandBuilder().Build());

        }

        public static async Task SlashCommandExecutedHandler(SocketSlashCommand command) {
            if (!SlashCommands.ContainsKey(command.Data.Name)) return;
            await SlashCommands[command.Data.Name].Execute(command);
        }

    }
}
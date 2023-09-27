using Discord;
using Discord.WebSocket;
using ServerStatusDiscordBot.Commands;
using ServerStatusDiscordBot.Configurations;
using ServerStatusDiscordBot.Exceptions;


namespace ServerStatusDiscordBot {
    class Program {

        private readonly DiscordSocketClient _client;

        public Program(){
         _client = new DiscordSocketClient();
        }

        static void Main() {
             new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() {
            Settings();
            _client.Log += LogHandler;
            _client.Ready += ReadyHandler;
            _client.SlashCommandExecuted += SlashCommandManager.SlashCommandExecutedHandler;
            await _client.LoginAsync(TokenType.Bot, Setting.Data.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private void Settings() {
            try {
                Setting.Setup();
                Console.WriteLine("Bot [ServerStatusDiscordBot] を起動します...");
            } catch (ConfigVerificationException e) {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            } catch {
                Console.WriteLine("settings.iniが破損しているか、存在していません。");
                Environment.Exit(1);
            }
        }

         private Task LogHandler(LogMessage message) {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }


        private async Task ReadyHandler() {
            SlashCommandManager.RegisterSlashCommand(_client);
            await _client.SetGameAsync("Let's Join Universe's world!");
        }

    }
}

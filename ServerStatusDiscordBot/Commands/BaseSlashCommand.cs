using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ServerStatusDiscordBot.Commands {
    public abstract class BaseSlashCommand {

        public abstract string Name { get; }

        /// <summary>
        ///     コマンドビルダー
        /// </summary>
        public abstract SlashCommandBuilder CommandBuilder();

        /// <summary>
        ///     実行
        /// </summary>
        /// <param name="command"></param>
        public abstract Task Execute(SocketSlashCommand command);
    }
}
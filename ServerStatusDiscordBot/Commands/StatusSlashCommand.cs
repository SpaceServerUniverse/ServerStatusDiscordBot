using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace ServerStatusDiscordBot.Commands {
    public class StatusSlashCommand : BaseSlashCommand {

        public override string Name => "status";

        public override SlashCommandBuilder CommandBuilder() {
            var slashCommandBuilder = new SlashCommandBuilder();
            slashCommandBuilder.Name = "status";
            slashCommandBuilder.Description = "現在の設定を確認します。";

            return slashCommandBuilder;
        }

        public async override Task Execute(SocketSlashCommand command) {
            await command.RespondAsync(embed: GetEmbed().Result.Build());
        }

        private async Task<EmbedBuilder> GetEmbed() {
            var embed = new EmbedBuilder();
            embed.WithTitle("Universe");
            embed.WithDescription("");
            embed.WithColor(new Color(16761035u));
            embed.WithTimestamp(DateTime.Now);
            var json = "";
            using (var httpClient = new HttpClient()) {
                var response = await httpClient.GetAsync("https://api.mcsrvstat.us/2/139.180.192.202");
                json = await response.Content.ReadAsStringAsync();

            }

            var jObject = JObject.Parse(json);
            if (!(bool)jObject["online"]) {
                embed.AddField("現在の状況", "オフライン", true);
                return embed;
            }

            var online = jObject["players"]["online"].ToString();
            var max = jObject["players"]["max"].ToString();
            var user = "現在はいません";
            if (int.Parse(online) > 0) {
                if (jObject["players"]["list"] == null) {
                    user = "エラーで取得できませんでした。";
                } else {
                    user = "";
                    foreach (var item in jObject["players"]["list"]) {
                        user += item + "\n";
                    }
                }
            }

            embed.AddField("現在の状況", "オンライン");
            embed.AddField("サーバー内人数", online + " / " + max);
            embed.AddField("ユーザー", user);
            return embed;
        }
    }
}
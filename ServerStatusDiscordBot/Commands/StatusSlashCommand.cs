using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using ServerStatusDiscordBot.Configurations;

namespace ServerStatusDiscordBot.Commands
{
    public class StatusSlashCommand : BaseSlashCommand
    {

        public override string Name => "status";

        public override SlashCommandBuilder CommandBuilder()
        {
            var slashCommandBuilder = new SlashCommandBuilder();
            slashCommandBuilder.Name = "status";
            slashCommandBuilder.Description = "現在のサーバーの状態を確認します。";

            return slashCommandBuilder;
        }

        public async override Task Execute(SocketSlashCommand command)
        {
            await command.RespondAsync(embed: GetEmbed().Result.Build());
        }

        private async Task<EmbedBuilder> GetEmbed()
        {
            var ip = Setting.Data.ServerIP;
            var port = Setting.Data.ServerPort;
            var embed = new EmbedBuilder();
            embed.WithTitle(Setting.Data.ServerName);
            embed.WithDescription("現在のサーバーの状態");
            embed.WithColor(new Color(16761035u));
            embed.WithTimestamp(DateTime.Now);
            var json = "";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://api.mcsrvstat.us/2/" + ip + ":" + port);
                json = await response.Content.ReadAsStringAsync();

            }

            var jObject = JObject.Parse(json);
            if (!(bool)jObject["online"])
            {
                embed.AddField("現在の状況", "オフライン", true);
                return embed;
            }

            var online = jObject["players"]["online"].ToString();
            var max = jObject["players"]["max"].ToString();
            var user = "現在はいません";
            if (int.Parse(online) > 0)
            {
                if (jObject["players"]["list"] == null && jObject["info"] == null)
                {
                    user = "エラーで取得できませんでした。";
                }
                else
                {
                    user = "";
                    if (jObject["players"]["list"] != null)
                    {
                        foreach (var item in jObject["players"]["list"])
                        {
                            user += item + "\n";
                        }
                    }
                    //Geyser BE
                    if (jObject["info"] != null)
                    {
                        foreach (var item in jObject["info"]["raw"])
                        {
                            user += item + "\n";
                        }
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
using ServerStatusDiscordBot.Exceptions;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ServerStatusDiscordBot.Configurations {
    public class Setting
    {
        public static AppSettingsConfig Data { get; set; }

        public static void Setup() {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddIniFile("settings.ini", optional: false);
            var configuration = builder.Build();
            Data = configuration.Get<AppSettingsConfig>();
            Verification();
        }

        private static void Verification() {
            if (Data.Token == string.Empty) throw new ConfigVerificationException("Tokenが入力されていないか、settings.iniが壊れています。");
        }
    }
}

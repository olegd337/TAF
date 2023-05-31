using NUnit.Framework;
using System.Configuration;

namespace Tools
{
    public class ConfigReader
    {
        private static AppSettingsSection _appSettingsSection;
        private static AppSettingsSection SettingsSection
        {
            get
            {
                if (_appSettingsSection == null)
                    Init();
                return _appSettingsSection;
            }
        }

        public static string Url => SettingsSection.Settings["Url"].Value;
        public static string User => SettingsSection.Settings["User"].Value;
        public static string Password => SettingsSection.Settings["Password"].Value;

        private static void Init()
        {
            var fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = "app.config"
            };

            try
            {
                var appConfig = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                _appSettingsSection = appConfig.GetSection("appSettings") as AppSettingsSection;
            }
            catch (Exception e)
            {
                Assert.Fail($"Fail is happened with message: {e.Message}");
            }
        }
    }
}
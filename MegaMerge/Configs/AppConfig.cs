using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


using Microsoft.Extensions.Configuration;

namespace MegaMerge.Configs
{
    public class AppConfig
    {
        private static AppConfig _current = BuildAppConfig();

        public static AppConfig Current
        {
            get => _current;
            private set => _current = value;
        }

        private static readonly OnAppConfigReloadDelegate OnAppConfigReload
            = new OnAppConfigReloadDelegate(new Action<AppConfig>((newConfig) => { })); // initialize

        public delegate void OnAppConfigReloadDelegate(AppConfig newConfig);

        private AppConfig() { }

        private static IConfigurationRoot _configurationRoot;

        public static IConfigurationRoot ConfigurationRoot => _configurationRoot ?? (_configurationRoot = BuildConfigurationRoot());

        private static IConfigurationRoot BuildConfigurationRoot()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            var appSettingJsonFile = Path.Combine(basePath, "appSettings.json");

            if (!File.Exists(appSettingJsonFile))
            {
                throw new FileNotFoundException("appSettings.json is not found in assembly folder.");
            }

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appSettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddJsonFile(
                    $"appSettings.{environment}.json",
                    optional: true,
                    reloadOnChange: true);

            var config = configBuilder.Build();

            return config;
        }

        private static AppConfig BuildAppConfig()
        {
            var appConfig = new AppConfig();

            ConfigurationRoot.Bind(appConfig);

            ConfigurationRoot
                .GetReloadToken()
                .RegisterChangeCallback(
                    (state) =>
                    {
                        Task
                            .Delay(5000)
                            .ContinueWith((t, o) =>
                            {
                                var config = BuildAppConfig();
                                AppConfig.Current = config;
                                OnAppConfigReload(config);
                            }, null);
                    },
                    null);

            return appConfig;
        }

        // Add properties from here when new configurations are added
        public InputConfig InputConfig { get; set; }
        public OutputConfig OutputConfig { get; set; }
    }
}

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MegaMerge.Configs;
using MegaMerge.Dtos;
using MegaMerge.Processors;
using MegaMerge.Processors.Interfaces;
using MegaMerge.Repository;
using MegaMerge.Repository.Interfaces;
using MegaMerge.Services;
using MegaMerge.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using FileType = MegaMerge.EnumsAndConstants.FileType;

namespace MegaMerge
{
    public class Program
    {
        public delegate IFileConsumeService ConsumeServiceResolver(FileType fileType);

        public static Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => ConfigureServices(services))
                .Build();
            var appConfig = AppConfig.Current;
            var inputConsumer = host.Services.GetService<IInputProcessService>();
            var outputGenerator = host.Services.GetService<IOutputGenerateService<MergedProduct>>();
            inputConsumer?.ProcessInput(appConfig.InputConfig);
            outputGenerator?.GenerateOutput();
            Console.WriteLine("Consume files completed.");
            return host.RunAsync();
        }

        // Do dependency injections here
        private static void ConfigureServices(IServiceCollection services)
        {
            // NLog config
            LogManager.Configuration = new NLogLoggingConfiguration(AppConfig.ConfigurationRoot.GetSection("NLog"));
            services.AddLogging(
                builder =>
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("NToastNotify", LogLevel.Warning)
                        .AddConsole();
                });

            services.AddSingleton<IDataRepository, DataRepository>();
            services.AddScoped<ICsvProcessor, CsvProcessor>();
            services.AddSingleton<IInputProcessService, InputProcessService>();
            services.AddTransient<CatalogConsumeService>();
            services.AddTransient<BarcodeConsumeService>();
            services.AddTransient<SupplierConsumeService>();
            services.AddTransient<ConsumeServiceResolver>(serviceProvider =>
                fileType => fileType switch
                {
                    FileType.Catalog => serviceProvider.GetService<CatalogConsumeService>(),
                    FileType.Barcode => serviceProvider.GetService<BarcodeConsumeService>(),
                    FileType.Supplier => serviceProvider.GetService<SupplierConsumeService>(),
                    _ => throw new NotSupportedException()
                });

            services.AddSingleton<IOutputGenerateService<MergedProduct>, OutputGenerateService>();
        }
    }
}

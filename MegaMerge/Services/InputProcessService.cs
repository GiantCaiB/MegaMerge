using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MegaMerge.Configs;
using MegaMerge.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ConsumeServiceResolver = MegaMerge.Program.ConsumeServiceResolver;

namespace MegaMerge.Services
{
    public class InputProcessService : IInputProcessService
    {
        private readonly ILogger<InputProcessService> _logger;
        private readonly ConsumeServiceResolver _consumeServiceResolver;

        public InputProcessService(
            ILogger<InputProcessService> logger,
            ConsumeServiceResolver consumeServiceResolver)
        {
            _logger = logger;
            _consumeServiceResolver = consumeServiceResolver;
        }

        public void ProcessInput(InputConfig inputConfig)
        {
            try
            {
                // Validations for inputConfig
                if (inputConfig == null)
                {
                    throw new ArgumentException("Null argument received.");
                }
                // Input folder must exist
                if (!Directory.Exists(inputConfig.FolderPath))
                {
                    throw new ArgumentException("Input folder path doesn't exist.");
                }
                // Enough information for file names
                if (inputConfig.FileDictionary.Count == 0)
                {
                    throw new ArgumentException("File dictionary is empty.");
                }

                // Loop companies
                inputConfig.FileDictionary.ToList().ForEach(companyInfo =>
                {
                    // Loop file names
                    companyInfo.Value.ToList().ForEach(fileInfo =>
                    {
                        var filePaths = new List<string>();
                        foreach (var filename in fileInfo.Value.ToList())
                        {
                            var filePath = Path.Combine(inputConfig.FolderPath, $"{filename}{EnumsAndConstants.InputFileFormat}");
                            if (!File.Exists(filePath))
                            {
                                _logger.LogWarning($"Skip the file: {filePath} because it is not found.");
                                continue;
                            }

                            filePaths.Add(filePath);
                        }
                        _consumeServiceResolver(fileInfo.Key).ConsumeFile(filePaths, companyInfo.Key);
                    });
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred when consume from {JsonConvert.SerializeObject(inputConfig, Formatting.None)}");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using MegaMerge.Processors.Interfaces;
using Microsoft.Extensions.Logging;

namespace MegaMerge.Processors
{
    public class CsvProcessor : ICsvProcessor
    {
        private readonly ILogger<CsvProcessor> _logger;
        private readonly CsvConfiguration _configuration;

        public CsvProcessor(ILogger<CsvProcessor> logger)
        {
            _logger = logger;
            _configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToUpper(),
            }; ;
        }
        public List<T> ReadCsvFile<T>(string filePath)
        {
            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, _configuration);
                return csv.GetRecords<T>().ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred when try to read file: {filePath}.");
                return new List<T>();
            }
        }

        public void WriteCsvFile<T>(string filePath, IEnumerable<T> records)
        {
            try
            {
                using var writer = new StreamWriter(filePath);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(records);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred when try to write file: {filePath}.");
            }
        }
    }
}

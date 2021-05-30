using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MegaMerge.Configs;
using MegaMerge.Dtos;
using MegaMerge.Extensions;
using MegaMerge.Processors.Interfaces;
using MegaMerge.Repository.Interfaces;
using MegaMerge.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace MegaMerge.Services
{
    public class OutputGenerateService : IOutputGenerateService<MergedProduct>
    {
        private readonly ILogger<OutputGenerateService> _logger;
        private readonly ICsvProcessor _csvProcessor;
        private readonly IDataRepository _repository;

        public OutputGenerateService(ILogger<OutputGenerateService> logger,
            ICsvProcessor csvProcessor,
            IDataRepository repository)
        {
            _logger = logger;
            _csvProcessor = csvProcessor;
            _repository = repository;
        }
        public IEnumerable<MergedProduct> AssembleObject()
        {
            var mappedProducts = new List<MergedProduct>();

            var catalogs = _repository.GetAllCatalogs();
            var barcodes = _repository.GetAllBarcodes();
            var suppliers = _repository.GetAllSuppliers();

            foreach (var catalog in catalogs)
            {
                try
                {
                    var filteredBarcodes = barcodes.Where(x =>
                        x.Source == catalog.Source
                        && x.Sku == catalog.Sku).ToList();
                    var supplier = suppliers.Single(x =>
                        x.Source == catalog.Source
                        && x.Id == filteredBarcodes.First().SupplierId);
                    mappedProducts.Add(new MergedProduct
                    {
                        Sku = catalog.Sku,
                        Source = catalog.Source,
                        Barcodes = filteredBarcodes,
                        SupplierName = supplier.Name,
                        Description = catalog.Description
                    });
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, $"Incomplete relationship found on catalog: {catalog.Sku} from company {catalog.Source}, skip for now.");
                }
            }
            return mappedProducts;
        }

        public void GenerateOutput()
        {
            try
            {
                var mergedProducts = AssembleObject().ToList();
                var filteredProducts = mergedProducts.RemoveDuplicates().ToList();
                _repository.InsertMergedProducts(filteredProducts);
                // write to file
                var outputConfig = AppConfig.Current.OutputConfig;
                if (!Directory.Exists(outputConfig.FolderPath))
                {
                    Directory.CreateDirectory(outputConfig.FolderPath);
                }
                var outputFilePath = Path.Combine(outputConfig.FolderPath, outputConfig.FileName);
                _csvProcessor.WriteCsvFile(outputFilePath, filteredProducts);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred when try to write the outputs to file.");
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegaMerge.Dtos;
using MegaMerge.Extensions;
using MegaMerge.Processors.Interfaces;
using MegaMerge.Repository.Interfaces;
using MegaMerge.Services.Interfaces;

namespace MegaMerge.Services
{
    public class BarcodeConsumeService : IFileConsumeService
    {
        private readonly ICsvProcessor _csvProcessor;
        private readonly IDataRepository _repository;

        public BarcodeConsumeService(
            IDataRepository repository,
            ICsvProcessor csvProcessor)
        {
            _repository = repository;
            _csvProcessor = csvProcessor;
        }
        public void ConsumeFile(IEnumerable<string> filePaths, string companyCode)
        {
            foreach (var filePath in filePaths)
            {
                var records = _csvProcessor.ReadCsvFile<SupplierProductBarcode>(filePath);
                // Mark the source on each records
                records = records.Select(x =>
                {
                    x.Source = companyCode;
                    return x;
                }).ToList();
                _repository.InsertBarcodes(records);
            }
        }
    }
}

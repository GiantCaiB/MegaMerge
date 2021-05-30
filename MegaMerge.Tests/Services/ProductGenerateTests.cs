using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegaMerge.Dtos;
using MegaMerge.Processors.Interfaces;
using MegaMerge.Repository.Interfaces;
using MegaMerge.Services;
using MegaMerge.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace MegaMerge.Tests.Services
{
    [TestClass]
    public class ProductGenerateTests
    {
        private readonly ILogger<OutputGenerateService> _logger;
        private readonly ICsvProcessor _csvProcessor;
        private readonly Mock<IDataRepository> _repositoryMock;
        private readonly IOutputGenerateService<MergedProduct> _outputGenerateService;

        public ProductGenerateTests()
        {
            _logger = Mock.Of<ILogger<OutputGenerateService>>();
            _csvProcessor = Mock.Of<ICsvProcessor>();
            _repositoryMock = new Mock<IDataRepository>();
            _outputGenerateService = new OutputGenerateService(_logger, _csvProcessor, _repositoryMock.Object);
        }

        [TestMethod]
        public void AssembleObject_AreEqual_WhenGiveDataFromRepository()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetAllCatalogs())
                .Returns(new List<Catalog>
                {
                    new Catalog
                    {
                        Sku = "647-vyk-317",
                        Description = "Walkers Special Old Whiskey",
                        Source = "A"
                    },
                    new Catalog
                    {
                        Sku = "999-eol-949",
                        Description = "Cheese - Grana Padano",
                        Source = "B"
                    }
                });
            _repositoryMock.Setup(x => x.GetAllBarcodes())
                .Returns(new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 1,
                        Sku = "647-vyk-317",
                        Barcode = "z2783613083819",
                        Source = "A"
                    },
                    new SupplierProductBarcode
                    {
                        SupplierId = 4,
                        Sku = "999-eol-949",
                        Barcode = "x0126648261918",
                        Source = "B"
                    }
                });
            _repositoryMock.Setup(x => x.GetAllSuppliers())
                .Returns(new List<Supplier>
                {
                    new Supplier
                    {
                       Id = 1,
                       Name = "Twitterbridge",
                       Source = "A"
                    },
                    new Supplier
                    {
                        Id = 4,
                        Name = "Bluejam",
                        Source = "B"
                    }
                });
            var expectedProducts = new List<MergedProduct>
            {
                new MergedProduct
                {
                    Barcodes = new List<SupplierProductBarcode>
                    {
                        new SupplierProductBarcode
                        {
                            SupplierId = 1,
                            Sku = "647-vyk-317",
                            Barcode = "z2783613083819",
                            Source = "A"
                        }
                    },
                    Description = "Walkers Special Old Whiskey",
                    IsDuplicated = false,
                    Sku = "647-vyk-317",
                    Source = "A",
                    SupplierName = "Twitterbridge"
                },
                new MergedProduct
                {
                    Barcodes = new List<SupplierProductBarcode>
                    {
                        new SupplierProductBarcode
                        {
                            SupplierId = 4,
                            Sku = "999-eol-949",
                            Barcode = "x0126648261918",
                            Source = "B"
                        }
                    },
                    Description = "Cheese - Grana Padano",
                    IsDuplicated = false,
                    Sku = "999-eol-949",
                    Source = "B",
                    SupplierName = "Bluejam"
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedProducts, Formatting.None);
            // Act
            var actualJson = JsonConvert.SerializeObject(_outputGenerateService.AssembleObject().ToList());
            // Assert
            Assert.AreEqual(expectedJson, actualJson);
        }
    }
}

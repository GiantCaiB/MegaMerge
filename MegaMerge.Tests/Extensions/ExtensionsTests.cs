using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegaMerge.Dtos;
using MegaMerge.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileType = MegaMerge.EnumsAndConstants.FileType;

namespace MegaMerge.Tests.Extensions
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetDtoType_ThrowException_WhenFileTypeIsUnknown()
        {
            // Arrange
            FileType filePath = FileType.Unknown;
            // Act
            var actualSource = filePath.GetDtoType();
        }

        [TestMethod]
        public void IsSameProduct_ReturnFalse_WhenSameSkuAndNoBarcodeSameAcross()
        {
            var sameSku = "647-vyk-317";
            var barcode1 = "c7417468772847";
            var barcode2 = "c7417468772848";
            // Arrange
            var source = new MergedProduct
            {
                Source = "A",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 1,
                        Sku = sameSku,
                        Barcode = barcode1,
                    },
                },
                SupplierName = "Supplier1",
                IsDuplicated = false,
                Description = "Des1"
            };

            var target = new MergedProduct
            {
                Source = "B",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 2,
                        Sku = sameSku,
                        Barcode = barcode2,
                    }
                },
                SupplierName = "Supplier2",
                IsDuplicated = false,
                Description = "Des2"

            };
            // Act
            var actualIsSameProduct = source.IsSameProduct(target);
            // Assert
            Assert.IsFalse(actualIsSameProduct);
        }

        [TestMethod]
        public void IsSameProduct_ReturnTrue_WhenBarcodeSameAcrossAndSkusAreDifferent()
        {
            var barcodeAcross = "c7417468772846";
            var sku1 = "647-vyk-317";
            var sku2 = "647-vyk-318";
            // Arrange
            var source = new MergedProduct
            {
                Source = "A",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 1,
                        Sku = sku1,
                        Barcode = barcodeAcross,
                    }
                },
                SupplierName = "Supplier1",
                IsDuplicated = false,
                Description = "Des1"
            };

            var target = new MergedProduct
            {
                Source = "B",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 2,
                        Sku = sku2,
                        Barcode = barcodeAcross,
                    }
                },
                SupplierName = "Supplier2",
                IsDuplicated = false,
                Description = "Des2"

            };
            // Act
            var actualIsSameProduct = source.IsSameProduct(target);
            // Assert
            Assert.IsTrue(actualIsSameProduct);
        }

        [TestMethod]
        public void RemoveDuplicates_AreEqual_WhenListOfProductsHasDuplicate()
        {
            // Arrange
            var barcodeAcross = "c7417468772846";
            var sku1 = "647-vyk-317";
            var sku2 = "647-vyk-318";
            var product1 = new MergedProduct
            {
                Source = "A",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 1,
                        Sku = sku1,
                        Barcode = barcodeAcross,
                    }
                },
                SupplierName = "Supplier1",
                IsDuplicated = false,
                Description = "Des1"
            };

            var product2 = new MergedProduct
            {
                Source = "B",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 2,
                        Sku = sku2,
                        Barcode = barcodeAcross,
                    }
                },
                SupplierName = "Supplier2",
                IsDuplicated = false,
                Description = "Des2"

            };
            var products = new List<MergedProduct> {product1, product2};
            // Act
            var filterProduct = products.RemoveDuplicates();
            // Assert
            Assert.AreEqual(filterProduct.Single(), product1);
        }

        [TestMethod]
        public void RemoveDuplicates_AreEqual_WhenListOfProductsHasNoDuplicate()
        {
            // Arrange
            var barcode1 = "c7417468772847";
            var barcode2 = "c7417468772848";
            var sku1 = "647-vyk-317";
            var sku2 = "647-vyk-318";
            var product1 = new MergedProduct
            {
                Source = "A",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 1,
                        Sku = sku1,
                        Barcode = barcode1,
                    }
                },
                SupplierName = "Supplier1",
                IsDuplicated = false,
                Description = "Des1"
            };

            var product2 = new MergedProduct
            {
                Source = "B",
                Barcodes = new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode
                    {
                        SupplierId = 2,
                        Sku = sku2,
                        Barcode = barcode2,
                    }
                },
                SupplierName = "Supplier2",
                IsDuplicated = false,
                Description = "Des2"

            };
            var products = new List<MergedProduct> { product1, product2 };
            // Act
            var filterProduct = products.RemoveDuplicates().ToList();
            // Assert
            CollectionAssert.AreEqual( products, filterProduct);
        }
    }
}

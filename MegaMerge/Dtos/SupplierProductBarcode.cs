using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace MegaMerge.Dtos
{
    public class SupplierProductBarcode : InputDto
    {
        [Name("SUPPLIERID")]
        public int SupplierId { get; set; }
        [Name("SKU")]
        public string Sku { get; set; }
        [Name("BARCODE")]
        public string Barcode { get; set; }
    }
}

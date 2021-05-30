using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace MegaMerge.Dtos
{
    public class MergedProduct
    {
        [Name("SKU")]
        public string Sku { get; set; }
        [Ignore]
        public string SupplierName { get; set; }
        [Name("Description")]
        public string Description { get; set; }
        [Name("Source")]
        public string Source { get; set; }
        [Ignore]
        public bool IsDuplicated { get; set; } = false;
        [Ignore]
        public ICollection<SupplierProductBarcode> Barcodes { get; set; }
    }
}

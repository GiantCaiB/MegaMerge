using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace MegaMerge.Dtos
{
    public class Catalog : InputDto
    {
        [Name("SKU")]
        public string Sku { get; set; }
        [Name("DESCRIPTION")]
        public string Description { get; set; }
    }
}

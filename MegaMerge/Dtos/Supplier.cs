using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace MegaMerge.Dtos
{
    public class Supplier : InputDto
    {
        [Name("ID")]
        public int Id { get; set; }
        [Name("NAME")]
        public string Name { get; set; }
    }
}

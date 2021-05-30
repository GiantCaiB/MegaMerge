using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace MegaMerge.Dtos
{
    public abstract class InputDto
    {
        [Ignore]
        public string Source { get; set; }
    }
}

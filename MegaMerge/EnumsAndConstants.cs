using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace MegaMerge
{
    public static class EnumsAndConstants
    {
        public const string InputFileFormat = ".csv";

        [JsonConverter(typeof(StringEnumConverter))]
        public enum FileType
        {
            Unknown = 0,
            Catalog = 1,
            Barcode = 2,
            Supplier = 3
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using FileType = MegaMerge.EnumsAndConstants.FileType;

namespace MegaMerge.Configs
{
    public class InputConfig
    {
        public string FolderPath { get; set; }
        public IDictionary<string, IDictionary<FileType, string[]>> FileDictionary { get; set; }
    }
}

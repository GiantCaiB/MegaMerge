using System;
using System.Collections.Generic;
using System.Text;

namespace MegaMerge.Processors.Interfaces
{
    public interface ICsvProcessor
    {
        List<T> ReadCsvFile<T>(string filePath);
        void WriteCsvFile<T>(string filePath, IEnumerable<T> records);
    }
}

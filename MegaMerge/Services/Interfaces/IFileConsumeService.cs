using System;
using System.Collections.Generic;
using System.Text;

namespace MegaMerge.Services.Interfaces
{
    public interface IFileConsumeService
    {
        void ConsumeFile(IEnumerable<string> filePaths, string companyCode);
    }
}

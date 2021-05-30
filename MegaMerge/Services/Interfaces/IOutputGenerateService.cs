using System;
using System.Collections.Generic;
using System.Text;
using MegaMerge.Dtos;

namespace MegaMerge.Services.Interfaces
{
    public interface IOutputGenerateService<T>
    { 
        void GenerateOutput();
        IEnumerable<T> AssembleObject();
    }
}

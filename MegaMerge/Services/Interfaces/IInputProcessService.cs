using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MegaMerge.Configs;

namespace MegaMerge.Services.Interfaces
{
    public interface IInputProcessService
    {
        void ProcessInput(InputConfig inputConfig);
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Access
{
    interface IInitArray
    {
        Task WriteArrayBeginning(CancellationToken token, string path);
    }
}

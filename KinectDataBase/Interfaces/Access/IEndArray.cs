using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Access
{
    public interface IEndArray
    {
        Task WriteArrayEnd(CancellationToken token, string path);
    }
}

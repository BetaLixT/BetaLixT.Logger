using System;
using System.Collections.Generic;
using System.Text;

namespace BetaLixT.Logger
{
    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; }

        public void Dispose()
        { }
    }
}

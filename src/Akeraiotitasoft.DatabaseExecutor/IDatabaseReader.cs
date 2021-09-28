using System;
using System.Collections.Generic;
using System.Text;

namespace Akeraiotitasoft.DatabaseExecutor
{
    public interface IDatabaseReader : IDisposable
    {
        List<T> ReadList<T>() where T : class, new();
    }
}

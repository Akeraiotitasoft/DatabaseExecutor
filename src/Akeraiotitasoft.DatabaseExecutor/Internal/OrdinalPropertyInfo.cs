using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Akeraiotitasoft.DatabaseExecutor.Internal
{
    internal class OrdinalPropertyInfo
    {
        public OrdinalPropertyInfo(PropertyInfo propertyInfo, int ordinal)
        {
            PropertyInfo = propertyInfo;
            Ordinal = ordinal;
        }

        public PropertyInfo PropertyInfo { get; private set; }

        public int Ordinal { get; private set; }
    }
}

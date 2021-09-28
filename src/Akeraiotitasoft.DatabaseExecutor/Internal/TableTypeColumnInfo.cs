using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Akeraiotitasoft.DatabaseExecutor.Internal
{
    internal class TableTypeColumnInfo
    {
        public TableTypeColumnInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            TableTypeColumnAttribute = propertyInfo.GetCustomAttribute<TableTypeColumnAttribute>();
        }

        public PropertyInfo PropertyInfo { get; private set; }

        public TableTypeColumnAttribute TableTypeColumnAttribute { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Akeraiotitasoft.DatabaseExecutor
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableTypeColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public string SqlDbType { get; set; }

        public long Length { get; set; }

        public byte Precision { get; set; }

        public byte Scale { get; set; }

        public int Order { get; set; }
    }
}

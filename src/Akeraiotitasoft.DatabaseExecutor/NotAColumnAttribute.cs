using System;
using System.Collections.Generic;
using System.Text;

namespace Akeraiotitasoft.DatabaseExecutor
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotAColumnAttribute : Attribute
    {
    }
}

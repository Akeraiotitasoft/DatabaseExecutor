using System;
using System.Collections.Generic;
using System.Text;

namespace Akeraiotitasoft.DatabaseExecutor
{
    [Serializable]
    public class DatabaseException : Exception
    {
        public DatabaseException() : base("Database error") { }

        public DatabaseException(string message) : base(message) { }

        public DatabaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}

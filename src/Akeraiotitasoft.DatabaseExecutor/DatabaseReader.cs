using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using Akeraiotitasoft.DatabaseExecutor.Internal;

namespace Akeraiotitasoft.DatabaseExecutor
{
    public class DatabaseReader : IDatabaseReader
    {
        private IDbConnection _connection = null;
        private IDbCommand _command = null;
        private IDataReader _reader = null;
        private bool _readOneList = false;

        internal DatabaseReader(IDbConnection connection, IDbCommand command, IDataReader reader)
        {
            _connection = connection;
            _command = command;
            _reader = reader;
        }

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public List<T> ReadList<T>()
            where T : class, new()
        {
            if (_readOneList)
            {
                RequireNextResult(_reader);
            }
            List<T> list = CopyValuesToList<T>(_reader);
            _readOneList = true;
            return list;
        }

        private static List<T> CopyValuesToList<T>(IDataReader dataReader)
            where T : class, new()
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            List<OrdinalPropertyInfo> ordinalProperties;
            string rememberTheName = null;
            try
            {
                ordinalProperties = (
                                        from property in properties
                                        where property.GetCustomAttribute<NotAColumnAttribute>() == null
                                        select new OrdinalPropertyInfo(property, dataReader.GetOrdinal(rememberTheName = property.Name))
                                    ).ToList();
            }
            catch (IndexOutOfRangeException indexOutOfRangeException)
            {
                throw new DatabaseException($"The property name {rememberTheName} was not found in the data reader.", indexOutOfRangeException);
            }

            List<T> list = new List<T>();
            while (dataReader.Read()) list.Add(CopyValuesToRecord<T>(dataReader, ordinalProperties));
            return list;
        }

        private static T CopyValuesToRecord<T>(IDataReader dataReader, List<OrdinalPropertyInfo> ordinalProperties)
            where T : class, new()
        {
            T record = new T();
            foreach (var ordinalProperty in ordinalProperties)
            {
                PropertyInfo property = ordinalProperty.PropertyInfo;
                int ordinal = ordinalProperty.Ordinal;
                if (!dataReader.IsDBNull(ordinal))
                {
                    object sqlValue = dataReader.GetValue(ordinal);
                    object recordValue;
                    try
                    {
                        recordValue = Convert.ChangeType(sqlValue, property.PropertyType);
                    }
                    catch (Exception excpetion)
                    {
                        throw new DatabaseException($"Error converting {sqlValue.GetType()} to {property.PropertyType}", excpetion);
                    }
                    property.SetValue(record, recordValue);
                }
            }
            return record;
        }

        private static void RequireNextResult(IDataReader dataReader)
        {
            if (!dataReader.NextResult())
            {
                throw new DatabaseException("Next Result was expected but was not found");
            }
        }
    }
}

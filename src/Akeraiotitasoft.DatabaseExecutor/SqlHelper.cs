using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Akeraiotitasoft.DatabaseExecutor.Internal;
using Microsoft.SqlServer.Server;

namespace Akeraiotitasoft.DatabaseExecutor
{
    public static class SqlHelper
    {
        public static SqlParameter CreateParameter(string parameterName, SqlDbType sqlDbType, object value)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameterName;
            sqlParameter.SqlDbType = sqlDbType;
            if (value == null)
            {
                sqlParameter.Value = DBNull.Value;
            }
            else
            {
                sqlParameter.Value = value;
            }
            return sqlParameter;
        }


        public static SqlParameter CreateParameter(string parameterName, SqlDbType sqlDbType, int size, object value)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameterName;
            sqlParameter.SqlDbType = sqlDbType;
            sqlParameter.Size = size;
            if (value == null)
            {
                sqlParameter.Value = DBNull.Value;
            }
            else
            {
                switch (sqlDbType)
                {
                    case SqlDbType.NVarChar:
                    case SqlDbType.VarChar:
                    case SqlDbType.Char:
                    case SqlDbType.NChar:
                        {
                            string strValue = value.ToString();
                            if (strValue.Length > size)
                            {
                                strValue = strValue.Substring(0, size);
                            }
                            sqlParameter.Value = strValue;
                        }
                        break;
                    default:
                        sqlParameter.Value = value;
                        break;
                }
            }
            return sqlParameter;
        }

        public static SqlParameter CreateStructuredParameter(string parameterName, object value)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameterName;
            sqlParameter.SqlDbType = SqlDbType.Structured;
            Type type = value.GetType();
            if (type == typeof(DataTable) || type.IsAssignableFrom(typeof(DbDataReader)))
            {
                sqlParameter.Value = value;
            }
            else if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type genericArgument = type.GetGenericArguments()[0];
                if (genericArgument == typeof(SqlDataRecord))
                {
                    sqlParameter.Value = value;
                }
                else
                {
                    sqlParameter.Value = typeof(SqlHelper)
                        .GetMethod("ConvertToDataRecords", BindingFlags.NonPublic | BindingFlags.Static)
                        .MakeGenericMethod(genericArgument)
                        .Invoke(null, new object[] { value });
                }
            }
            else
            {
                throw new DatabaseException("Can only convert IEnumerable<T> and use DataTable directly");
            }

            return sqlParameter;
        }

        public static SqlParameter CreateStructuredParameter<T>(string parameterName, IEnumerable<T> value)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameterName;
            sqlParameter.SqlDbType = SqlDbType.Structured;
            sqlParameter.Value = ConvertToDataRecords<T>(value);
            return sqlParameter;
        }


        private static IEnumerable<SqlDataRecord> ConvertToDataRecords<T>(IEnumerable<T> enumerable)
        {
            Type type = typeof(T);
            if (type.IsAssignableFrom(typeof(SqlDataRecord)))
            {
                foreach (var record in enumerable)
                {
                    yield return (SqlDataRecord)(object)record;
                }
                yield break; // exit
            }
            var allProperties =
                from property in typeof(T).GetProperties()
                select new TableTypeColumnInfo(property);
            List<TableTypeColumnInfo> properties =
                (from column in allProperties
                 where column.TableTypeColumnAttribute != null
                 orderby column.TableTypeColumnAttribute.Order
                 select column).ToList();
            SqlMetaData[] sqlMetaDatas =
                (from property in properties
                 select CreateSqlMetaData(type, property)).ToArray();
            SqlDataRecord sqlDataRecord = new SqlDataRecord(sqlMetaDatas);
            foreach (T t in enumerable)
            {
                PopulateDataRecord(sqlDataRecord, properties, t);
                yield return sqlDataRecord;
            }
        }

        private static SqlMetaData CreateSqlMetaData(Type type, TableTypeColumnInfo column)
        {
            var property = column.PropertyInfo;
            var attribute = column.TableTypeColumnAttribute;
            if (string.IsNullOrWhiteSpace(attribute.Name))
            {
                throw new DatabaseException($"TableTypeColumnAttribute for property {property.Name} does not have a name in class {type.Name}");
            }
            SqlDbType sqlDbType;
            if (!Enum.TryParse<SqlDbType>(attribute.SqlDbType, true, out sqlDbType))
            {
                throw new DatabaseException($"TableTypeColumnAttribute {attribute.Name} for property {property.Name} has a bad SqlDbType {attribute.SqlDbType} in class {type.Name}");
            }
            switch(sqlDbType)
            {
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.Binary:
                case SqlDbType.VarBinary:
                    return new SqlMetaData(attribute.Name, sqlDbType, attribute.Length);
                case SqlDbType.Structured:
                    throw new DatabaseException("Cannot nest a structured parameter in a structured parameter");
                case SqlDbType.Decimal:
                    return new SqlMetaData(attribute.Name, sqlDbType, attribute.Precision, attribute.Scale);
                case SqlDbType.Float:
                    return new SqlMetaData(attribute.Name, sqlDbType, attribute.Precision);
                default:
                    return new SqlMetaData(attribute.Name, sqlDbType);
            }
        }

        private static void PopulateDataRecord<T>(SqlDataRecord sqlDataRecord, List<TableTypeColumnInfo> columns, T t)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                sqlDataRecord.SetValue(i, columns[i].PropertyInfo.GetValue(t, null));
            }
        }
    }
}

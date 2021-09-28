using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

using Akeraiotitasoft.DisposableTuple;

namespace Akeraiotitasoft.DatabaseExecutor
{
    public class DatabaseExecutor : IDatabaseExecutor
    {
        public List<T> ExecuteStoredProcedure<T>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T : class, new()
        {
            using var reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
            return reader.ReadList<T>();
        }

        public Tuple<List<T1>, List<T2>> ExecuteStoredProcedure<T1, T2>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
        {
            using var reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
            return Tuple.Create(
                reader.ReadList<T1>(),
                reader.ReadList<T2>());
        }

        public Tuple<List<T1>, List<T2>, List<T3>> ExecuteStoredProcedure<T1, T2, T3>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            using var reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
            return Tuple.Create(
                reader.ReadList<T1>(), 
                reader.ReadList<T2>(),
                reader.ReadList<T3>());
        }

        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> ExecuteStoredProcedure<T1, T2, T3, T4>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
        {
            using var reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
            return Tuple.Create(
                reader.ReadList<T1>(),
                reader.ReadList<T2>(),
                reader.ReadList<T3>(),
                reader.ReadList<T4>());
        }

        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> ExecuteStoredProcedure<T1, T2, T3, T4, T5>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new()
        {
            using var reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
            return Tuple.Create(
                reader.ReadList<T1>(),
                reader.ReadList<T2>(),
                reader.ReadList<T3>(),
                reader.ReadList<T4>(),
                reader.ReadList<T5>());
        }

        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T6>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new()
            where T6 : class, new()
        {
            using var reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
            return Tuple.Create(
                reader.ReadList<T1>(),
                reader.ReadList<T2>(),
                reader.ReadList<T3>(),
                reader.ReadList<T4>(),
                reader.ReadList<T5>(),
                reader.ReadList<T6>());
        }

        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T6, T7>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new()
            where T6 : class, new()
            where T7 : class, new()
        {
            using var reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
            return Tuple.Create(
                reader.ReadList<T1>(),
                reader.ReadList<T2>(),
                reader.ReadList<T3>(),
                reader.ReadList<T4>(),
                reader.ReadList<T5>(),
                reader.ReadList<T6>(),
                reader.ReadList<T7>());
        }

        public DisposableTuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, TDatabaseReader> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T6, T7, TDatabaseReader>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new()
            where T6 : class, new()
            where T7 : class, new()
            where TDatabaseReader : IDatabaseReader
        {
            IDatabaseReader reader = null;
            try
            {
                reader = ExecuteStoredProcedure(connectionString, storedProcedureName, args);
                return new DisposableTuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, TDatabaseReader>(
                    reader.ReadList<T1>(),
                    reader.ReadList<T2>(),
                    reader.ReadList<T3>(),
                    reader.ReadList<T4>(),
                    reader.ReadList<T5>(),
                    reader.ReadList<T6>(),
                    reader.ReadList<T7>(),
                    (TDatabaseReader)reader);
            }
            catch
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                throw;
            }
        }

        public IDatabaseReader ExecuteStoredProcedure(string connectionString, string storedProcedureName, params IDataParameter[] args)
        {
            IDbConnection connection = null;
            IDbCommand command = null;
            IDataReader reader = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                foreach (IDataParameter parameter in args) command.Parameters.Add(parameter);
                reader = command.ExecuteReader();
                return new DatabaseReader(connection, command, reader);
            }
            catch
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                if (command != null)
                {
                    command.Dispose();
                }
                if (connection != null)
                {
                    connection.Dispose();
                }
                throw;
            }
        }

        public int ExecuteStoredProcedureNonQuery(string connectionString, string storedProcedureName, params IDataParameter[] args)
        {
            using IDbConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using IDbCommand command = connection.CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            foreach (IDataParameter parameter in args) command.Parameters.Add(parameter);
            return command.ExecuteNonQuery();
        }
    }
}


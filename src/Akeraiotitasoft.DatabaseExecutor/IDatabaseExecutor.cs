using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Akeraiotitasoft.DisposableTuple;

namespace Akeraiotitasoft.DatabaseExecutor
{
    public interface IDatabaseExecutor
    {
        List<T> ExecuteStoredProcedure<T>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T : class, new();

        Tuple<List<T1>, List<T2>> ExecuteStoredProcedure<T1, T2>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new();

        Tuple<List<T1>, List<T2>, List<T3>> ExecuteStoredProcedure<T1, T2, T3>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new();

        Tuple<List<T1>, List<T2>, List<T3>, List<T4>> ExecuteStoredProcedure<T1, T2, T3, T4>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new();

        Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> ExecuteStoredProcedure<T1, T2, T3, T4, T5>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new();

        Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T6>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new()
            where T6 : class, new();

        Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T6, T7>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new()
            where T6 : class, new()
            where T7 : class, new();

        DisposableTuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, TDatabaseReader> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T6, T7, TDatabaseReader>(string connectionString, string storedProcedureName, params IDataParameter[] args)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
            where T5 : class, new()
            where T6 : class, new()
            where T7 : class, new()
            where TDatabaseReader : IDatabaseReader;

        IDatabaseReader ExecuteStoredProcedure(string connectionString, string storedProcedureName, params IDataParameter[] args);

        int ExecuteStoredProcedureNonQuery(string connectionString, string storedProcedureName, params IDataParameter[] args);
    }
}

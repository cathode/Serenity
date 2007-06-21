using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Data
{
    /// <summary>
    /// Used to determine which database is used for any data operations.
    /// </summary>
    public enum DatabaseScope
    {
        Global,
        Environment,
        Module,
    }
    public static class Database
    {
        public static QueryResultSet ExecuteQuery(string query)
        {
            return Database.ExecuteQuery(query, DatabaseScope.Global);
        }
        public static QueryResultSet ExecuteQuery(string query, DatabaseScope scope)
        {

        }
    }
    public sealed class QueryResultSet
    {

    }
    public sealed class QueryResultRow
    {

    }
}

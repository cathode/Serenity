/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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

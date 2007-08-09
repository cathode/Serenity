/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
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

/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SQLite;
using System.Text;
using System.IO;
using Serenity.IO;
using System.Linq;

namespace Serenity.Data
{
    /// <summary>
    /// Provides access to fast local data stores.
    /// </summary>
    public static class Database
    {
        #region Methods - Public
        /// <summary>
        /// Creates the database of the specified scope if it does not exist.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static bool Create(DataScope scope)
        {
            if (Database.IsCreated(scope))
            {
                return true;
            }
            else
            {
                var datafilePath = Database.GetDatafilePath(scope);
                var schemaPaths = Database.GetSchemaPaths(scope);
                SQLiteConnection.CreateFile(datafilePath);
                if (schemaPaths != null && schemaPaths.Count() > 0)
                {
                    SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder();
                    csb.DataSource = datafilePath;
                    csb.DateTimeFormat = SQLiteDateFormats.ISO8601;
                    csb.Pooling = true;

                    SQLiteConnection conn = new SQLiteConnection(csb.ConnectionString);
                    conn.Open();
                    foreach (string path in schemaPaths)
                    {
                        SQLiteCommand cmd = new SQLiteCommand(File.ReadAllText(path), conn);

                        cmd.ExecuteNonQuery();
                        //Does cmd need to be disposed as well?
                        cmd.Dispose();
                    }
                    conn.Close();
                    conn.Dispose();

                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Creates and returns a connection to the database of the specified scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static SQLiteConnection Connect(DataScope scope)
        {
            if (!Database.IsCreated(scope))
            {
                Database.Create(scope);
            }
            var builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = Database.GetDatafilePath(scope);
            builder.Pooling = true;
            builder.DateTimeFormat = SQLiteDateFormats.ISO8601;

            return new SQLiteConnection(builder.ConnectionString);
        }
        /// <summary>
        /// Determines if the database of the specified scope has been created.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static bool IsCreated(DataScope scope)
        {
            return File.Exists(Database.GetDatafilePath(scope));
        }
        /// <summary>
        /// Gets the path of the datafile of the specified module and domain.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string GetDatafilePath(string module, string domain)
        {
            return Path.Combine(SerenityPath.DataDirectory,
                Path.Combine(Path.GetFileNameWithoutExtension(module),
                    Path.Combine(Path.GetFileNameWithoutExtension(domain), "database")));
        }
        /// <summary>
        /// Gets the path of the datafile of the specified scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static string GetDatafilePath(DataScope scope)
        {
            if (scope == DataScope.Global)
            {
                return Path.Combine(SerenityPath.DataDirectory, "global/database.s3db");
            }
            else if (scope == DataScope.Module)
            {
                if (Module.Current == null)
                {
                    return null;
                }
                return PathExt.Combine(SerenityPath.DataDirectory, "module",
                    Path.GetFileNameWithoutExtension(Module.Current.Name), "database.s3db");
            }
            else if (scope == DataScope.Domain)
            {
                if (Domain.Current == null)
                {
                    return null;
                }
                return PathExt.Combine(SerenityPath.DataDirectory, "domain",
                    Path.GetFileNameWithoutExtension(Domain.Current.HostName), "database.s3db");
            }
            else if (scope == DataScope.ModuleAndDomain)
            {
                if (Domain.Current == null || Module.Current == null)
                {
                    return null;
                }
                return PathExt.Combine(SerenityPath.DataDirectory, "module",
                    Path.GetFileNameWithoutExtension(Module.Current.Name),
                    Path.GetFileNameWithoutExtension(Domain.Current.HostName), "database.s3db");
            }
            else
            {
                throw new ArgumentException(__Strings.Exceptions.UnrecognizedDataScope);
            }
        }
        /// <summary>
        /// Gets the path of the database schema file for the specified scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetSchemaPaths(DataScope scope)
        {
            if (scope == DataScope.Global)
            {
                string dir = PathExt.Combine(SerenityPath.DataDirectory, "global", "schemas");
                if (Directory.Exists(dir))
                {
                    return from p in Directory.GetFiles(dir)
                           where Path.GetExtension(p) == ".sql"
                           select p;
                }
            }
            else if (scope == DataScope.Module || scope == DataScope.ModuleAndDomain)
            {
                if (Module.Current != null)
                {
                    string dir = PathExt.Combine(SerenityPath.DataDirectory, "module",
                               Path.GetFileNameWithoutExtension(Module.Current.Name), "schemas");
                    if (Directory.Exists(dir))
                    {
                        return from p in Directory.GetFiles(dir)
                               where Path.GetExtension(p) == ".sql"
                               select p;
                    }
                }
            }
            else if (scope == DataScope.Domain)
            {
                if (Domain.Current != null)
                {
                    string dir = PathExt.Combine(SerenityPath.DataDirectory, "domain", "schemas");
                    if (Directory.Exists(dir))
                    {
                        return from p in Directory.GetFiles(dir)
                               where Path.GetExtension(p) == ".sql"
                               select p;
                    }
                }
            }
            else
            {
                throw new ArgumentException(__Strings.Exceptions.UnrecognizedDataScope);
            }
            return null;
        }
        #endregion
    }
}

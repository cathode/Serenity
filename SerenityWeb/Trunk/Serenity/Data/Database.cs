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

namespace Serenity.Data
{
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
                string datafilePath = Database.GetDatafilePath(scope);
                string schemaPath = Database.GetSchemaPath(scope);
                SQLiteConnection.CreateFile(datafilePath);
                if (File.Exists(schemaPath))
                {
                    SQLiteConnection conn = new SQLiteConnection("Data Source=" + datafilePath);
                    SQLiteCommand cmd = new SQLiteCommand(File.ReadAllText(schemaPath), conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Clone();
                    conn.Dispose();
                    //Does cmd need to be disposed as well?
                    cmd.Dispose();
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

            return new SQLiteConnection("Data Source=" + Database.GetDatafilePath(scope));
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
        /// Gets the path of the global datafile.
        /// </summary>
        /// <returns></returns>
        public static string GetDatafilePath()
        {
            return Path.Combine(SerenityPath.DataDirectory, "global/database");
        }
        /// <summary>
        /// Gets the path of the datafile of the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static string GetDatafilePath(string module)
        {
            return Path.Combine(SerenityPath.DataDirectory,
                Path.Combine(Path.GetFileNameWithoutExtension(module), "database"));
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
                return Database.GetDatafilePath();
            }
            else if (scope == DataScope.Module)
            {
                if (Module.Current == null)
                {
                    return null;
                }

                return Database.GetDatafilePath(Module.Current.Name);
            }
            else if (scope == DataScope.Domain)
            {
                if (Domain.Current == null || Module.Current == null)
                {
                    return null;
                }
                return Database.GetDatafilePath(Module.Current.Name, Domain.Current.HostName);
            }
            else
            {
                throw new ArgumentException(__Strings.Exceptions.UnrecognizedDataScope);
            }
        }
        /// <summary>
        /// Gets the path of the database schema file for the global scope.
        /// </summary>
        /// <returns></returns>
        public static string GetSchemaPath()
        {
            return Path.Combine(SerenityPath.DataDirectory, "global/schema.sql");
        }
        /// <summary>
        /// Gets the path of the database schema file for the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static string GetSchemaPath(string module)
        {
            return Path.Combine(SerenityPath.DataDirectory,
                Path.Combine(Path.GetFileNameWithoutExtension(module), "schema.sql"));
        }
        /// <summary>
        /// Gets the path of the database schema file for the specified scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static string GetSchemaPath(DataScope scope)
        {
            if (scope == DataScope.Global)
            {
                return Database.GetSchemaPath();
            }
            else if (scope == DataScope.Module)
            {
                if (Module.Current == null)
                {
                    return null;
                }

                return Database.GetSchemaPath(Module.Current.Name);
            }
            else if (scope == DataScope.Domain)
            {
                if (Domain.Current == null || Module.Current == null)
                {
                    return null;
                }
                return Database.GetSchemaPath(Module.Current.Name);
            }
            else
            {
                throw new ArgumentException(__Strings.Exceptions.UnrecognizedDataScope);
            }
        }
        #endregion
    }
}

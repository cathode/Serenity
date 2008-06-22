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

namespace Serenity.Data
{
    public static class Database
    {
        private const string GlobalDBPath = "./data/global/db";
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
            else if (scope == DataScope.Global)
            {
                SQLiteConnection.CreateFile(Database.GlobalDBPath);
                if (File.Exists(Database.GlobalDBPath + ".sql"))
                {
                    SQLiteConnection conn = new SQLiteConnection("DataSource=" + Database.GlobalDBPath);
                    SQLiteCommand cmd = new SQLiteCommand(File.ReadAllText(Database.GlobalDBPath + ".sql"), conn);

                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            //TODO: Implement support for multiple scopes.
            else if (scope == DataScope.Module)
            {
                if (Module.Current == null)
                {
                    return false;
                }
                throw new NotImplementedException();
            }
            else if (scope == DataScope.Domain)
            {
                if (Domain.Current == null)
                {
                    return false;
                }
                throw new NotImplementedException();
            }
            else
            {
                throw new ArgumentException(__Strings.Exceptions.UnrecognizedDataScope);
            }
        }
        /// <summary>
        /// Creates and returns a connection to the database of the specified scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static SQLiteConnection Connect(DataScope scope)
        {
            if (!Database.IsCreated(scope) && !Database.Create(scope))
            {
                return null;
            }
            //TODO: Implement support for multiple scopes.
            if (scope == DataScope.Global)
            {
                var csb = new SQLiteConnectionStringBuilder();
                csb.DataSource = Database.GlobalDBPath;
                return new SQLiteConnection(csb.ConnectionString);
            }
            else if (scope == DataScope.Module)
            {
                if (Module.Current == null)
                {
                    return null;
                }
                throw new NotImplementedException();
            }
            else if (scope == DataScope.Domain)
            {
                if (Domain.Current == null)
                {
                    return null;
                }
                throw new NotImplementedException();
            }
            else
            {
                throw new ArgumentException(__Strings.Exceptions.UnrecognizedDataScope);
            }
        }
        public static bool IsCreated(DataScope scope)
        {
            //TODO: Implement support for multiple scopes.
            if (scope == DataScope.Global)
            {
                return File.Exists(Database.GlobalDBPath);
            }
            else if (scope == DataScope.Module)
            {
                if (Module.Current == null)
                {
                    return false;
                }
                throw new NotImplementedException();
            }
            else if (scope == DataScope.Domain)
            {
                if (Domain.Current == null)
                {
                    return false;
                }
                throw new NotImplementedException();
            }
            else
            {
                throw new ArgumentException(__Strings.Exceptions.UnrecognizedDataScope);
            }
        }
    }
}

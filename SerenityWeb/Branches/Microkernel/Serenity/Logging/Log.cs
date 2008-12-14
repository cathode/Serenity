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
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Data.SQLite;

using Serenity.Data;

namespace Serenity.Logging
{
    /// <summary>
    /// Provides a method which allows other parts of Serenity or
    /// loaded modules to write messages to a central log store.
    /// </summary>
    public static class Log 
    {
        #region Methods - Public
        /// <summary>
        /// Logs a message to the log store.
        /// </summary>
        /// <param name="message">The message of the logged entry.</param>
        /// <remarks>
        /// The default <see cref="Severity"/> used is <see cref="Severity.Info"/>.
        /// </remarks>
        public static void RecordEvent(string message)
        {
            Log.RecordEvent(message, Severity.Info, null);
        }
        /// <summary>
        /// Logs a message to the log store.
        /// </summary>
        /// <param name="message">The message of the logged entry.</param>
        /// <param name="severity">How severe the entry is. See the <see cref="Severity"/> enum for more info.</param>
        public static void RecordEvent(string message, Severity severity)
        {
            Log.RecordEvent(message, severity, null);
        }
        /// <summary>
        /// Logs a message to the log store
        /// </summary>
        /// <param name="message">The message of the logged entry.</param>
        /// <param name="severity">How severe the entry is. See the <see cref="Severity"/> enum for more info.</param>
        /// <param name="debugInfo">Information used to debug the cause of the logged entry. Usually used if an error occured.</param>
        public static void RecordEvent(string message, Severity severity, string debugInfo)
        {
            var conn = Database.Connect(DataScope.Global);
            Guid eventId = Guid.NewGuid();
            var cmd = new SQLiteCommand(string.Format("INSERT INTO 'log' ('event_id', 'message', 'severity', 'debug', 'assembly') VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                eventId.ToString(),
                message,
                ((int)severity).ToString(),
                debugInfo,
                null), conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        #endregion
    }
    
}
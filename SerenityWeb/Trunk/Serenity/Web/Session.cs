using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using Serenity.Data;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a client session, and allows state data to be persisted between requests.
    /// </summary>
    public sealed class Session : IDisposable
    {
        #region Constructors - Private
        private Session()
        {
            this.sessionID = Guid.NewGuid();

        }
        private Session(Guid sessionID)
        {
            this.sessionID = sessionID;
        }
        #endregion
        #region Fields - Private
        private DateTime created;
        private bool isDisposed;
        private TimeSpan lifetime;
        private DateTime modified;
        private static readonly SessionCollection pool = new SessionCollection();
        private readonly Guid sessionID;
        #endregion
        #region Fields - Public
        public const int DefaultLifetime = 300000;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Removes all session information.
        /// </summary>
        public static void ClearAll()
        {
            var conn = Database.Connect(DataScope.Global);
            conn.EnsureOpen();
            var cmd = new SQLiteCommand("DELETE FROM sessions", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("DELETE FROM session_data", conn);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Disposes the current <see cref="Session"/>, but does not remove the session information from the database.
        /// </summary>
        public void Dispose()
        {
            this.isDisposed = true;

            lock (Session.pool)
            {
                Session.pool.Remove(this);
            }
            GC.SuppressFinalize(this);
        }
        //public static Session GetSession(Request request)
        //{
            
        //}
        /// <summary>
        /// Gets a stored <see cref="Session"/>.
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public static Session GetSession(Guid sessionID)
        {
            lock (Session.pool)
            {
                if (Session.pool.Contains(sessionID))
                {
                    return Session.pool[sessionID];
                }
            }

            var con = Database.Connect(DataScope.Global);
            con.EnsureOpen();

            var cmd = new SQLiteCommand("SELECT (created, modified, lifetime) FROM sessions WHERE id == "
                + sessionID.ToString("N") + " LIMIT 1", con);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    object o = reader["created"];
                    Session s = new Session(sessionID);

                    lock (Session.pool)
                    {
                        Session.pool.Add(s);
                    }
                    return s;
                }
                else
                {
                    //session expired
                    return null;
                }
            }
        }
        /// <summary>
        /// Creates and returns a new <see cref="Session"/>.
        /// </summary>
        /// <returns>
        /// This method adds the newly created session information into the database before returning.
        /// </returns>
        public static Session NewSession()
        {
            Session s = new Session();

            s.created = DateTime.Now;
            s.modified = s.created;
            s.lifetime = TimeSpan.FromMilliseconds(Session.DefaultLifetime);
            var connection = Database.Connect(DataScope.Global);

            var cmd = new SQLiteCommand(string.Format("INSERT INTO sessions('id', 'created', 'lifetime', 'modified') VALUES('{0}','{1}','{2}','{3}')",
                s.SessionID.ToString("N"),
                s.created.ToString("s"),
                s.lifetime.TotalMilliseconds.ToString(),
                s.modified.ToString("s")), connection);

            connection.EnsureOpen();

            if (cmd.ExecuteNonQuery() == 1)
            {
                lock (Session.pool)
                {
                    Session.pool.Add(s);
                }
                return s;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Reads a value from the current session.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ReadValue(string name)
        {
            var connection = Database.Connect(DataScope.Global);
            var cmd = new SQLiteCommand(string.Format("SELECT value FROM session_data WHERE id == '{0}' AND name == '{1}'",
                this.SessionID.ToString("N"),
                name), connection);
            connection.EnsureOpen();

            return cmd.ExecuteScalar() as string;
        }
        public static void Remove(Guid sessionID)
        {
            var s = Session.GetSession(sessionID);
            var connection = Database.Connect(DataScope.Global);
            if (s != null)
            {
                connection.EnsureOpen();

                var cmd = new SQLiteCommand(string.Format("DELETE FROM sessions WHERE id == '{0}'",
                    sessionID.ToString("N")), connection);
                cmd.ExecuteNonQuery();

                cmd = new SQLiteCommand(string.Format("DELETE FROM session_data WHERE id == '{0}'",
                    sessionID.ToString("N")), connection);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Removes a value associated with the current <see cref="Session"/>
        /// </summary>
        /// <param name="name"></param>
        public void RemoveValue(string name)
        {
            var connection = Database.Connect(DataScope.Global);
            connection.EnsureOpen();
            var cmd = new SQLiteCommand(string.Format("DELETE FROM session_data WHERE id == '{0}' AND name == '{1}'",
                this.SessionID.ToString("N"),
                name), connection);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Writes a value to the current session.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteValue(string name, string value)
        {
            var connection = Database.Connect(DataScope.Global);
            connection.EnsureOpen();
            var cmd = new SQLiteCommand(string.Format("INSERT INTO session_data VALUES('{0}', '{1}', '{2}')",
                this.sessionID.ToString("N"),
                name,
                value), connection);
            cmd.ExecuteNonQuery();
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Session"/> has been disposed.
        /// </summary>
        /// <remarks>
        /// Disposing refers to the in-memory <see cref="Session"/> object only.
        /// Disposing a session does not remove the session data from the database.
        /// </remarks>
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }
        /// <summary>
        /// Gets the <see cref="Guid"/> associated with the current session.
        /// </summary>
        public Guid SessionID
        {
            get
            {
                return this.sessionID;
            }
        }
        #endregion
        #region Types - Private
        private sealed class SessionCollection : KeyedCollection<Guid, Session>
        {
            protected override Guid GetKeyForItem(Session item)
            {
                return item.SessionID;
            }
        }
        #endregion
    }
}

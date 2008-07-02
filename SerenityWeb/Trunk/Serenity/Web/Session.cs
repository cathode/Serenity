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
        private readonly Guid sessionID;
        private DateTime created;
        private TimeSpan lifetime;
        private DateTime modified;
        private SQLiteConnection connection;
        private bool isDisposed;
        private static readonly SessionCollection pool = new SessionCollection();
        #endregion
        #region Fields - Public
        public const int DefaultLifetime = 300000;
        #endregion
        #region Methods - Public
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
            if (this.connection != null)
            {
                this.connection.Dispose();
            }
            GC.SuppressFinalize(this);
        }
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

            var cmd = new SQLiteCommand("SELECT (created, modified, lifetime) FROM sessions WHERE id == "
                + sessionID.ToString("N") + " LIMIT 1", con);

            con.Open();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    object o = reader["created"];
                    Session s = new Session(sessionID);
                    s.connection = con;

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
        public static void Remove(Guid sessionID)
        {
            var s = Session.GetSession(sessionID);

            if (s != null)
            {
                var cmd = new SQLiteCommand(string.Format("DELETE FROM sessions WHERE id == '{0}'",
                    sessionID.ToString("N")), s.connection);

                s.connection.Open();
                cmd.ExecuteNonQuery();

                cmd = new SQLiteCommand(string.Format("DELETE FROM session_data WHERE id == '{0}'",
                    sessionID.ToString("N")), s.connection);

                cmd.ExecuteNonQuery();

                s.connection.Close();
            }
        }
        public static void ClearAll()
        {
            var cmd = new SQLiteCommand("DELETE FROM sessions", Database.Connect(DataScope.Global));
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("DELETE FROM session_data", cmd.Connection);
            cmd.ExecuteNonQuery();
        }
        public static Session NewSession()
        {
            Session s = new Session();

            s.created = DateTime.Now;
            s.modified = s.created;
            s.lifetime = TimeSpan.FromMilliseconds(Session.DefaultLifetime);
            s.connection = Database.Connect(DataScope.Global);

            var cmd = new SQLiteCommand(string.Format("INSERT INTO sessions VALUES('{0}','{1}','{2}','{3}')",
                s.SessionID.ToString("N"),
                s.created.ToString("s"),
                s.lifetime.TotalMilliseconds.ToString(),
                s.modified.ToString("s")), s.connection);

            s.connection.Open();

            if (cmd.ExecuteNonQuery() == 1)
            {
                lock (Session.pool)
                {
                    Session.pool.Add(s);
                }
                s.connection.Close();
                return s;
            }
            else
            {
                s.connection.Close();
                return null;
            }
        }
        /// <summary>
        /// Writes a value to the current session.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteValue(string name, string value)
        {
            this.connection.Open();
            var cmd = new SQLiteCommand(string.Format("INSERT INTO session_data VALUES('{0}', '{1}', '{2}')",
                this.sessionID.ToString("N"),
                name,
                value), this.connection);

            cmd.ExecuteNonQuery();

            this.connection.Close();
        }
        /// <summary>
        /// Reads a value from the current session.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ReadValue(string name)
        {
            var cmd = new SQLiteCommand(string.Format("SELECT value FROM session_data WHERE id == '{0}' AND name == '{1}'",
                this.SessionID.ToString("N"),
                name), this.connection);

            this.connection.Open();
            string result = cmd.ExecuteScalar() as string;
            this.connection.Close();

            return result;
        }
        public void RemoveValue(string name)
        {
            var cmd = new SQLiteCommand(string.Format("DELETE FROM session_data WHERE id == '{0}' AND name == '{1}'",
                this.SessionID.ToString("N"),
                name), this.connection);

            this.connection.Open();
            cmd.ExecuteNonQuery();
            this.connection.Close();
        }
        #endregion
        #region Properties - Public
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

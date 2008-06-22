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
        /// Disposes the current <see cref="Session"/>.
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

        public static Session NewSession()
        {
            Session s = new Session();

            s.created = DateTime.Now;
            s.modified = s.created;
            s.lifetime = TimeSpan.FromMilliseconds(Session.DefaultLifetime);
            s.connection = Database.Connect(DataScope.Global);

            var cmd = new SQLiteCommand("INSERT INTO sessions VALUES(" + s.SessionID.ToString("N")
                + ", " + s.created.ToString("s") + ", " + s.lifetime.Milliseconds.ToString() + ")", s.connection);

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
            throw new NotImplementedException();
        }
        /// <summary>
        /// Reads a value from the current session.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ReadValue(string name)
        {
            throw new NotImplementedException();
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

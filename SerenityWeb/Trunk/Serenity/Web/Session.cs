using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a client session, and allows state data to be persisted between requests.
    /// </summary>
    public sealed class Session
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
        #endregion
        #region Methods - Public
        /// <summary>
        /// Destroys the current <see cref="Session"/>.
        /// </summary>
        public void Destroy()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets a stored <see cref="Session"/>.
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public static Session GetSession(Guid sessionID)
        {
            throw new NotImplementedException();
            /*
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.Pooling = true;
            sb.DataSource = "data/serenity.s3db";
            SQLiteConnection con = new SQLiteConnection(sb.ConnectionString);

            con.Open();


            con.Clone();

            con.Dispose();
            */
        }
        /// <summary>
        /// Writes a value to the current session.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteValue(string name, object value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Reads a value from the current session.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object ReadValue(string name)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the Globally Unique Identifier (GUID) associated with the current session.
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

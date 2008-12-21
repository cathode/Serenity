using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity
{
    public sealed class EventDetails
    {
        public EventDetails(string message)
        {
        }
        #region Fields
        private EventKind kind;
        private string message;
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();
        private string stackTrace;
        #endregion
        #region Properties
        public Dictionary<string, object> Data
        {
            get
            {
                return this.data;
            }
        }
        public EventKind Kind
        {
            get
            {
                return this.kind;
            }
            set
            {
                this.kind = value;
            }
        }
        public string Message
        {
            get
            {
                return this.message;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity
{
    public sealed class EventLog
    {
        #region Events
        public event EventHandler<EventRecordedEventArgs> EventRecorded;
        #endregion
        #region Methods
        public void RecordEvent(string message)
        {
            this.RecordEvent(new EventDetails(message));
        }
        public void RecordEvent(string message, EventKind kind)
        {
            this.RecordEvent(new EventDetails(message)
            {
                Kind = kind
            });
        }
        public void RecordEvent(string message, EventKind kind, string stackTrace)
        {
            this.RecordEvent(new EventDetails(message)
            {
                Kind = kind,
                StackTrace = stackTrace
            });
        }
        public void RecordEvent(EventDetails details)
        {
            if (this.EventRecorded != null)
            {
                this.EventRecorded(this, new EventRecordedEventArgs(details));
            }
        }
        #endregion
    }
}

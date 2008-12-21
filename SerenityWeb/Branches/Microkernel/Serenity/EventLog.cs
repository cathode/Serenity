using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity
{
    public class EventLog
    {
        #region Events
        public EventHandler<EventRecordedEventArgs> EventRecorded;
        #endregion
        #region Methods
        public void RecordEvent(string message)
        {
        }
        public void RecordEvent(string message, EventKind kind)
        {

        }
        public void RecordEvent(string message, EventKind kind, string stackTrace)
        {

        }
        public void RecordEvent(EventDetails details)
        {
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity
{
    public sealed class EventRecordedEventArgs : EventArgs
    {
        #region Constructors
        public EventRecordedEventArgs(EventDetails details)
        {
            this.details = details;
        }
        #endregion
        #region Fields
        private readonly EventDetails details;
        #endregion
        #region Properties
        public EventDetails Details
        {
            get
            {
                return this.details;
            }
        }
        #endregion
    }
}

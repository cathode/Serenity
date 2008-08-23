using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web;

namespace Serenity.Net
{
    /// <summary>
    /// Represents event data for the <see cref="ProtocolDriver.RequestAvailable"/> event.
    /// </summary>
    public sealed class RequestAvailableEventArgs : EventArgs
    {
        #region Constructors - Public
        public RequestAvailableEventArgs(Request request)
        {
            this.request = request;
        }
        #endregion
        #region Fields - Private
        private Request request;
        #endregion
        #region Properties - Public
        public Request Request
        {
            get
            {
                return this.request;
            }
        }
        #endregion
    }
}

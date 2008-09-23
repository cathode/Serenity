using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Serenity.Web;
using Serenity.Web.Resources;
using Serenity.Web.Forms;

namespace Serenity.Pages
{
    public sealed class EventViewer : DocumentResource
    {
        #region Constructors - Public
        public EventViewer()
        {
            this.Name = "EventViewer";
        }
        #endregion
        #region Methods - Protected
        protected override Document CreateForm()
        {
            return new EventViewerDocument();
        }
        #endregion
        #region Types - Internal
        internal sealed class EventViewerDocument : Document
        {
            internal EventViewerDocument()
            {
                this.eventsDisplay = new Table();

                

                this.Controls.Add(this.eventsDisplay);
            }
            private Table eventsDisplay;
        }
        #endregion
    }
}
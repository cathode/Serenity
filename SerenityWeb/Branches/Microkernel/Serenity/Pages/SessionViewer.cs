using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Serenity.Web.Forms;
using Serenity.Data;
using System.Data.SQLite;

namespace Serenity.Pages
{
    public sealed class SessionViewer : DocumentResource
    {
        public SessionViewer()
        {
            this.Name = "SessionViewer";
        }

        protected override Document CreateForm()
        {
            return new SessionViewerDocument();
        }

        internal sealed class SessionViewerDocument : Document
        {
            internal SessionViewerDocument()
            {
                this.Head.Title = "Session Viewer";

                this.Body.Controls.Add(new Heading(HeadingLevel.H1, "Session Viewer"));

                this.sessionView = new Table();
                this.sessionView.Controls.Add(
                    new TableRow(
                        new TableHeader("Session ID"),
                        new TableHeader("Created On"),
                        new TableHeader("Lifetime"),
                        new TableHeader("Last Modified On")));
                this.sessionView.PreRender += new EventHandler<RenderEventArgs>(sessionView_PreRender);

                this.Body.Controls.Add(this.sessionView);
            }

            void sessionView_PreRender(object sender, RenderEventArgs e)
            {
                var conn = Database.Connect(DataScope.Global);

                var cmd = new SQLiteCommand("SELECT * FROM 'sessions'", conn);

                conn.Open();
                var result = cmd.ExecuteReader();

                while (result.Read())
                {
                    this.sessionView.VolatileControls.Add(
                        new TableRow(
                            new TableCell(result["id"].ToString()),
                            new TableCell(result["created"].ToString()),
                            new TableCell(result["lifetime"].ToString()),
                            new TableCell(result["last_modified"].ToString())));
                }
            }
            #region Fields - Private
            private Table sessionView;
            #endregion
        }
    }
}

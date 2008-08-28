using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web;
using Serenity.Web.Resources;
using Serenity.Data;
using System.Data.SQLite;

namespace Serenity.Pages
{
    public sealed class ViewSessionsPage : DynamicResource
    {
        public ViewSessionsPage()
        {
            this.ContentType = MimeType.TextPlain;
            this.Name = "ViewSessions";
        }
        public override void OnRequest(Request request, Response response)
        {
            var conn = Database.Connect(DataScope.Global);

            var cmd = new SQLiteCommand("SELECT * FROM 'sessions'", conn);

            conn.Open();
            var result = cmd.ExecuteReader();

            while (result.Read())
            {
                response.WriteLine(string.Format("{0}, {1}, {2}, {3}", result["id"], result["created"], result["lifetime"], result["last_modified"]));
            }
        }
    }
}

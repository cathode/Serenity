using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a path to a web resource on a server. /WebApp/Resource
    /// </summary>
    public sealed class WebPath
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebPath"/> class.
        /// </summary>
        /// <param name="path"></param>
        public WebPath(string path)
        {
            var segs = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segs.Length > 0)
                this.WebApp = segs[0];

            if (segs.Length > 1)
            {
                this.segments = new string[segs.Length - 1];
                segs.CopyTo(this.segments, 1);
            }
        }
        public WebPath(string webApp, IList<string> resourceSubPathSegments)
        {
            this.WebApp = webApp;
            if (resourceSubPathSegments != null)
                this.segments = resourceSubPathSegments.ToArray();
        }
        #endregion
        #region Fields
        private string webApp = string.Empty;
        private string[] segments = new string[0];
        #endregion
        #region Properties
        public string WebApp
        {
            get
            {
                return this.webApp;
            }
            set
            {
                if (value == null)
                    this.webApp = string.Empty;
                else
                    this.webApp = value;
            }
        }
        public string ResourceSubPath
        {
            get
            {
                return string.Join("/", this.segments);
            }
        }
        public ReadOnlyCollection<string> ResourceSubPathSegments
        {
            get
            {
                return new ReadOnlyCollection<string>(this.segments);
            }
        }

        #endregion
        #region Methods
        public static WebPath FromString(string path)
        {
            return new WebPath(path);
        }
        public override string ToString()
        {
            if (this.segments.Length == 0)
                return "/";
            else
                return string.Join("/", this.segments);
        }
        #endregion
    }
}

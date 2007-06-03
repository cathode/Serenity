using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a MIME type, usually used to identify the type of content in a message.
    /// </summary>
    public struct MimeType
    {
        #region Constructors - Public
        public MimeType(string type, string subtype)
        {
            //WS: add input-validation here.
            this.type = type;
            this.subtype = subtype;
        }
        #endregion
        #region Fields - Private
        private string subtype;
        private string type;
        #endregion

        #region Properties - Public
        /// <summary>
        /// application/EDI-X12
        /// </summary>
        public static MimeType ApplicationEdiX12
        {
            get
            {
                return new MimeType("application", "EDI-X12");
            }
        }
        /// <summary>
        /// application/EDIFACT
        /// </summary>
        public static MimeType ApplicationEdiFact
        {
            get
            {
                return new MimeType("application", "EDIFACT");
            }
        }
        /// <summary>
        /// application/javascript
        /// </summary>
        public static MimeType ApplicationJavascript
        {
            get
            {
                return new MimeType("application", "javascript");
            }
        }
        /// <summary>
        /// application/octet-stream
        /// </summary>
        public static MimeType ApplicationOctetStream
        {
            get
            {
                return new MimeType("application", "octet-stream");
            }
        }
        /// <summary>
        /// application/ogg
        /// </summary>
        public static MimeType ApplicationOgg
        {
            get
            {
                return new MimeType("application", "ogg");
            }
        }
        /// <summary>
        /// application/xhtml+xml
        /// </summary>
        public static MimeType ApplicationXhtmlPlusXml
        {
            get
            {
                return new MimeType("application", "xhtml+xml");
            }
        }
        /// <summary>
        /// application/xml
        /// </summary>
        public static MimeType ApplicationXml
        {
            get
            {
                return new MimeType("application", "xml");
            }
        }
        /// <summary>
        /// application/x-shockwave-flash
        /// </summary>
        public static MimeType ApplicationXShockwaveFlash
        {
            get
            {
                return new MimeType("application", "x-shockwave-flash");
            }
        }
        /// <summary>
        /// audio/mpeg
        /// </summary>
        public static MimeType AudioMpeg
        {
            get
            {
                return new MimeType("audio", "mpeg");
            }
        }
        /// <summary>
        /// audio/x-ms-wma
        /// </summary>
        public static MimeType AudioXMSWma
        {
            get
            {
                return new MimeType("audio", "x-ms-wma");
            }
        }
        /// <summary>
        /// audio/vnd.rn-realaudio
        /// </summary>
        public static MimeType AudioVendorRNRealAudio
        {
            get
            {
                return new MimeType("audio", "vnd.rn-realaudio");
            }
        }
        /// <summary>
        /// audio/x-wav
        /// </summary>
        public static MimeType AudioXWav
        {
            get
            {
                return new MimeType("audio", "x-wav");
            }
        }
        /// <summary>
        /// image/gif
        /// </summary>
        public static MimeType ImageGif
        {
            get
            {
                return new MimeType("image", "gif");
            }
        }
        /// <summary>
        /// image/jpeg
        /// </summary>
        public static MimeType ImageJpeg
        {
            get
            {
                return new MimeType("image", "jpeg");
            }
        }
        /// <summary>
        /// image/png
        /// </summary>
        public static MimeType ImagePng
        {
            get
            {
                return new MimeType("image", "png");
            }
        }
        /// <summary>
        /// image/tiff
        /// </summary>
        public static MimeType ImageTiff
        {
            get
            {
                return new MimeType("image", "tiff");
            }
        }
        /// <summary>
        /// image/vnd.microsoft.icon
        /// </summary>
        public static MimeType ImageVendorMicrosoftIcon
        {
            get
            {
                return new MimeType("image", "vnd.microsoft.icon");
            }
        }
        /// <summary>
        /// multipart/mixed
        /// </summary>
        public static MimeType MultipartMixed
        {
            get
            {
                return new MimeType("multipart", "mixed");
            }
        }
        public static MimeType MultipartAlternative
        {
            get
            {
                return new MimeType("multipart", "alternative");
            }
        }
        public static MimeType MultipartRelated
        {
            get
            {
                return new MimeType("multipart", "related");
            }
        }
        #endregion
    }
}

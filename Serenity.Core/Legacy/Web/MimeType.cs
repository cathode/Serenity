﻿/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using Serenity.Properties;

namespace Serenity.Web
{
	/// <summary>
	/// Represents a MIME type, usually used to identify the type of content in a message.
	/// </summary>
	public struct MimeType
	{
		#region Constructors - Public
		/// <summary>
		/// Initializes a new instance of the MimeType class using the supplied type/subtype.
		/// </summary>
		/// <param name="type">The primary type of the new MimeType.</param>
		/// <param name="subtype">The secondary type of the new MimeType.</param>
		public MimeType(string type, string subtype)
		{
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            else if (type.Length == 0)
            {
                throw new ArgumentException(string.Format(AppResources.ParamEmptyException, "type"), "type");
            }
            else if (subtype == null)
            {
                throw new ArgumentNullException("subtype");
            }
            else if (subtype.Length == 0)
            {
                throw new ArgumentException(string.Format(AppResources.ParamEmptyException, "subtype"), "subtype");
            }

			this.type = type;
			this.subtype = subtype;
		}
		#endregion
		#region Fields - Private
        private readonly string subtype;
		private readonly string type;
		#endregion
        #region Fields - Public
        /// <summary>
        /// Holds the default <see cref="StringComparison"/> used to compare
        /// the values of <see cref="MimeType"/>s for equality tests.
        /// </summary>
        public const StringComparison DefaultComparison = StringComparison.OrdinalIgnoreCase;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Determines if two MimeType objects are equal to each other.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(MimeType a, MimeType b)
        {
            return MimeType.Equals(a, b, MimeType.DefaultComparison);
        }
        /// <summary>
        /// Determines if two MimeType objects are equal to each other.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool Equals(MimeType a, MimeType b, StringComparison comparison)
        {
            return (a.Type.Equals(b.Type, comparison) && a.Subtype.Equals(b.Subtype, comparison));
        }
        /// <summary>
        /// Determines if the current MimeType is equal to another MimeType.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Equals(MimeType value)
        {
            return MimeType.Equals(this, value, MimeType.DefaultComparison);
        }
        /// <summary>
        /// Determines if the current MimeType is equal to another MimeType.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public bool Equals(MimeType value, StringComparison comparison)
        {
            return MimeType.Equals(this, value, comparison);
        }
        /// <summary>
        /// Determines if the current MimeType is equal to another object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj.GetType().TypeHandle.Equals(typeof(MimeType).TypeHandle)))
            {
                return MimeType.Equals(this, (MimeType)obj, MimeType.DefaultComparison);
            }
            return false;
        }
        /// <summary>
        /// Determines if the current MimeType is equal to another object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public bool Equals(object obj, StringComparison comparison)
        {
            if ((obj != null) && (obj.GetType().TypeHandle.Equals(typeof(MimeType).TypeHandle)))
            {
                return MimeType.Equals(this, (MimeType)obj, comparison);
            }
            return false;
        }
		/// <summary>
		/// Creates a new MimeType given a supplied string in the format:
        /// "type/subtype"
		/// </summary>
		/// <param name="mimeType"></param>
		/// <returns></returns>
        public static MimeType FromString(string mimeType)
        {
            if (mimeType == null)
            {
                throw new ArgumentNullException("mimeType");
            }
            else if (mimeType.Length == 0)
            {
                throw new ArgumentException(string.Format(AppResources.ParamEmptyException, "mimeType"), "mimeType");
            }

            string[] parts = mimeType.Split('/');
            if (parts.Length != 2)
            {
                throw new ArgumentException(string.Format(AppResources.ParamEmptyException, "mimeType"));
            }

            return new MimeType(parts[0], parts[1]);
        }
        /// <summary>
        /// Overridden. Gets the hash code for the current MimeType.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode() ^ 0x0523f3cd;
        }
		/// <summary>
		/// Overridden. Returns a string representation of the current MimeType.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.type + "/" + this.subtype;
		}
		#endregion
        #region Operators - Public
        /// <summary>
        /// Compares two MimeType objects and determines if they are inequal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(MimeType a, MimeType b)
        {
            return !MimeType.Equals(a, b, MimeType.DefaultComparison);
        }
        /// <summary>
        /// Compares two MimeType objects and determines if they are equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(MimeType a, MimeType b)
        {
            return MimeType.Equals(a, b, MimeType.DefaultComparison);
        }
        #endregion
        #region Properties - Public
        /// <summary>
		/// Gets the secondary type associated with the current MimeType.
		/// </summary>
		public string Subtype
		{
			get
			{
				return this.subtype;
			}
		}
		/// <summary>
		/// Gets the primary type associated with the current MimeType.
		/// </summary>
		public string Type
		{
			get
			{
				return this.type;
			}
		}
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
		/// application/x-www-form-urlencoded
		/// </summary>
		public static MimeType ApplicationXWwwFormUrlEncoded
		{
			get
			{
				return new MimeType("application", "x-www-form-urlencoded");
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
        /// Gets the default <see cref="MimeType"/>.
        /// </summary>
		public static MimeType Default
		{
			get
			{
				return new MimeType("text", "plain");
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
		/// <summary>
		/// multipart/alternative
		/// </summary>
		public static MimeType MultipartAlternative
		{
			get
			{
				return new MimeType("multipart", "alternative");
			}
		}
		/// <summary>
		/// multipart/form-data
		/// </summary>
		public static MimeType MultipartFormData
		{
			get
			{
				return new MimeType("multipart", "form-data");
			}
		}
		/// <summary>
		/// multipart/related
		/// </summary>
		public static MimeType MultipartRelated
		{
			get
			{
				return new MimeType("multipart", "related");
			}
		}
		/// <summary>
		/// text/css
		/// </summary>
		public static MimeType TextCss
		{
			get
			{
				return new MimeType("text", "css");
			}
		}
		/// <summary>
		/// text/html
		/// </summary>
		public static MimeType TextHtml
		{
			get
			{
				return new MimeType("text", "html");
			}
		}
		/// <summary>
		/// text/javascript
		/// </summary>
		public static MimeType TextJavascript
		{
			get
			{
				return new MimeType("text", "javascript");
			}
		}
		/// <summary>
		/// text/plain
		/// </summary>
		public static MimeType TextPlain
		{
			get
			{
				return new MimeType("text", "plain");
			}
		}
		#endregion
	}
}
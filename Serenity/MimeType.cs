/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Diagnostics.Contracts;

namespace Serenity
{
    /// <summary>
    /// Represents a MIME type, usually used to identify the type of content in a message.
    /// </summary>
    public struct MimeType
    {
        #region Fields
        /// <summary>
        /// Holds the default <see cref="StringComparison"/> used to compare
        /// the values of <see cref="MimeType"/>s for equality tests.
        /// </summary>
        public const StringComparison DefaultComparison = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// Backing field for the <see cref="MimeType.Subtype"/> property.
        /// </summary>
        private readonly string subtype;

        /// <summary>
        /// Backing field for the <see cref="MimeType.Type"/> property.
        /// </summary>
        private readonly string type;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MimeType"/> struct.
        /// </summary>
        /// <param name="type">The primary type.</param>
        /// <param name="subtype">The secondary type.</param>
        public MimeType(string type, string subtype)
        {
            Contract.Requires(!string.IsNullOrEmpty(type));
            Contract.Requires(!string.IsNullOrEmpty(subtype));

            this.type = type;
            this.subtype = subtype;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the secondary type associated with the current MimeType.
        /// </summary>
        public string Subtype
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
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
                Contract.Ensures(Contract.Result<string>() != null);
                return this.type;
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/EDI-X12'.
        /// </summary>
        public static MimeType ApplicationEdiX12
        {
            get
            {
                return new MimeType("application", "EDI-X12");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/EDIFACT'.
        /// </summary>
        public static MimeType ApplicationEdiFact
        {
            get
            {
                return new MimeType("application", "EDIFACT");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/javascript'.
        /// </summary>
        public static MimeType ApplicationJavascript
        {
            get
            {
                return new MimeType("application", "javascript");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/octet-stream'.
        /// </summary>
        public static MimeType ApplicationOctetStream
        {
            get
            {
                return new MimeType("application", "octet-stream");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/ogg'.
        /// </summary>
        public static MimeType ApplicationOgg
        {
            get
            {
                return new MimeType("application", "ogg");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/xhtml+xml'.
        /// </summary>
        public static MimeType ApplicationXhtmlPlusXml
        {
            get
            {
                return new MimeType("application", "xhtml+xml");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/xml'.
        /// </summary>
        public static MimeType ApplicationXml
        {
            get
            {
                return new MimeType("application", "xml");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/x-shockwave-flash'.
        /// </summary>
        public static MimeType ApplicationXShockwaveFlash
        {
            get
            {
                return new MimeType("application", "x-shockwave-flash");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'application/x-www-form-urlencoded'.
        /// </summary>
        public static MimeType ApplicationXWwwFormUrlEncoded
        {
            get
            {
                return new MimeType("application", "x-www-form-urlencoded");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'audio/mpeg'.
        /// </summary>
        public static MimeType AudioMpeg
        {
            get
            {
                return new MimeType("audio", "mpeg");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'audio/x-ms-wma'.
        /// </summary>
        public static MimeType AudioXMSWma
        {
            get
            {
                return new MimeType("audio", "x-ms-wma");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'audio/vnd.rn-realaudio'.
        /// </summary>
        public static MimeType AudioVendorRNRealAudio
        {
            get
            {
                return new MimeType("audio", "vnd.rn-realaudio");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'audio/x-wav'.
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
        /// Gets a <see cref="MimeType"/> for 'image/gif'.
        /// </summary>
        public static MimeType ImageGif
        {
            get
            {
                return new MimeType("image", "gif");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'image/jpeg'.
        /// </summary>
        public static MimeType ImageJpeg
        {
            get
            {
                return new MimeType("image", "jpeg");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'image/png'.
        /// </summary>
        public static MimeType ImagePng
        {
            get
            {
                return new MimeType("image", "png");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'image/tiff'.
        /// </summary>
        public static MimeType ImageTiff
        {
            get
            {
                return new MimeType("image", "tiff");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'image/vnd.microsoft.icon'.
        /// </summary>
        public static MimeType ImageVendorMicrosoftIcon
        {
            get
            {
                return new MimeType("image", "vnd.microsoft.icon");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'multipart/mixed'.
        /// </summary>
        public static MimeType MultipartMixed
        {
            get
            {
                return new MimeType("multipart", "mixed");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'multipart/alternative'.
        /// </summary>
        public static MimeType MultipartAlternative
        {
            get
            {
                return new MimeType("multipart", "alternative");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'multipart/form-data'.
        /// </summary>
        public static MimeType MultipartFormData
        {
            get
            {
                return new MimeType("multipart", "form-data");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'multipart/related'.
        /// </summary>
        public static MimeType MultipartRelated
        {
            get
            {
                return new MimeType("multipart", "related");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'text/css'.
        /// </summary>
        public static MimeType TextCss
        {
            get
            {
                return new MimeType("text", "css");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'text/html'.
        /// </summary>
        public static MimeType TextHtml
        {
            get
            {
                return new MimeType("text", "html");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'text/javascript'.
        /// </summary>
        public static MimeType TextJavascript
        {
            get
            {
                return new MimeType("text", "javascript");
            }
        }

        /// <summary>
        /// Gets a <see cref="MimeType"/> for 'text/plain'.
        /// </summary>
        public static MimeType TextPlain
        {
            get
            {
                return new MimeType("text", "plain");
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Determines if two <see cref="MimeType"/> instances are equal.
        /// </summary>
        /// <param name="a">The first <see cref="MimeType"/> instance to compare.</param>
        /// <param name="b">The second <see cref="MimeType"/> instance to compare.</param>
        /// <returns>true if both instances represent the same value; otherwise, false.</returns>
        public static bool Equals(MimeType a, MimeType b)
        {
            return MimeType.Equals(a, b, MimeType.DefaultComparison);
        }

        /// <summary>
        /// Determines if two <see cref="MimeType"/> instances are equal.
        /// </summary>
        /// <param name="a">The first <see cref="MimeType"/> instance to compare.</param>
        /// <param name="b">The second <see cref="MimeType"/> instance to compare.</param>
        /// <param name="comparison">The kind of string comparison to use.</param>
        /// <returns>true if both instances represent the same value; otherwise, false.</returns>
        public static bool Equals(MimeType a, MimeType b, StringComparison comparison)
        {
            return a.Type.Equals(b.Type, comparison) && a.Subtype.Equals(b.Subtype, comparison);
        }

        /// <summary>
        /// Determines if the current MimeType is equal to another MimeType.
        /// </summary>
        /// <param name="value">The other <see cref="MimeType"/> instance to compare to.</param>
        /// <returns>true if the current instance represents the same value as the specified instance; otherwise, false.</returns>
        public bool Equals(MimeType value)
        {
            return MimeType.Equals(this, value, MimeType.DefaultComparison);
        }

        /// <summary>
        /// Determines if the current MimeType is equal to another MimeType.
        /// </summary>
        /// <param name="value">The other <see cref="MimeType"/> instance to compare to.</param>
        /// <param name="comparison">The kind of string comparison to use.</param>
        /// <returns>true if the current instance represents the same value as the specified instance; otherwise, false.</returns>
        public bool Equals(MimeType value, StringComparison comparison)
        {
            return MimeType.Equals(this, value, comparison);
        }

        /// <summary>
        /// Determines if the current MimeType is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>true if <paramref name="obj"/> is a <see cref="MimeType"/> and represents the same value as this instance.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType().TypeHandle.Equals(typeof(MimeType).TypeHandle))
                return MimeType.Equals(this, (MimeType)obj, MimeType.DefaultComparison);

            return false;
        }

        /// <summary>
        /// Parses the type and subtype of a string and constructs a new <see cref="MimeType"/> instance, which is returned.
        /// </summary>
        /// <param name="mimeType">A mime-type represented as a string to be parsed.</param>
        /// <returns>A new <see cref="MimeType"/> instance that is parsed from the specified string.</returns>
        public static MimeType Parse(string mimeType)
        {
            Contract.Requires(!string.IsNullOrEmpty(mimeType));

            string[] parts = mimeType.Split('/');

            if (parts.Length != 2)
                throw new FormatException(ExceptionMessages.BadMimeTypeFormat);

            return new MimeType(parts[0], parts[1]);
        }

        /// <summary>
        /// Overridden. Returns a string representation of the current <see cref="MimeType"/>.
        /// </summary>
        /// <returns>A string representation of the current instance.</returns>
        public override string ToString()
        {
            return this.type + "/" + this.subtype;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return (this.type.GetHashCode() + 0x2A4D) | (this.subtype.GetHashCode() + 0x922D);
        }

        /// <summary>
        /// Defines invariants for the <see cref="MimeType"/> struct.
        /// </summary>
        [ContractInvariantMethod]
        private void __InvariantMethod()
        {
            Contract.Invariant(this.type != null);
            Contract.Invariant(this.subtype != null);
        }
        #endregion
        #region Operators - Public
        /// <summary>
        /// Compares two MimeType objects and determines if they are inequal.
        /// </summary>
        /// <param name="a">The first <see cref="MimeType"/> instance to compare.</param>
        /// <param name="b">The second <see cref="MimeType"/> instance to compare.</param>
        /// <returns>true if both instances represent the different values; otherwise, false.</returns>
        public static bool operator !=(MimeType a, MimeType b)
        {
            return !MimeType.Equals(a, b, MimeType.DefaultComparison);
        }

        /// <summary>
        /// Compares two MimeType objects and determines if they are equal.
        /// </summary>
        /// <param name="a">The first <see cref="MimeType"/> instance to compare.</param>
        /// <param name="b">The second <see cref="MimeType"/> instance to compare.</param>
        /// <returns>true if both instances represent the same value; otherwise, false.</returns>
        public static bool operator ==(MimeType a, MimeType b)
        {
            return MimeType.Equals(a, b, MimeType.DefaultComparison);
        }
        #endregion
    }
}

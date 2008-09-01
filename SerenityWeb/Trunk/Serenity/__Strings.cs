/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    internal static class __Strings
    {
        #region Fields - Internal
        internal const string ArgumentCannotBeEmpty = "The supplied argument cannot be empty";
        internal const string CannotModifyWhileRunning = "Cannot modify server-wide objects while the server is in a running state";
        internal const string CannotModifyResourceMimetype = "Cannot set MimeType for this Resource kind";
        internal const string StreamMustSupportWriting = "Supplied stream must support writing";
        internal const string DomainNameAlreadyExists = "A Domain with the same name already exists";
        internal const string ModuleNameAlreadyExists = "A Module with the same name already exists";
        internal const string MustBeDirectoryResource = "The supplied path must be a directory path";
        internal const string CannotModifyRequestDataStream = "Cannot modify a RequestDataStream";
        internal const string CannotFlushRequestDataStream = "Cannot flush a RequestDataStream";
        #endregion
        #region Types - Internal
        internal static class Exceptions
        {
            #region Fields - Internal
            internal const string UnrecognizedDataScope = "Unrecognized value of scope paramater.";
            #endregion
        }
        #endregion
    }
}

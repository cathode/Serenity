/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Data;

namespace Serenity.Web
{
    /// <summary>
    /// Represents an error page/response that is returned to the client.
    /// </summary>
    public sealed class ErrorResource : DynamicResource
    {
        //TODO: Implement ErrorResource class.

        //public override void OnRequest(Request request, Response response)
        //{
        //    var conn = Database.Connect(DataScope.Domain);

        //    var cmd = conn.CreateCommand();
        //    cmd.CommandText = "SELECT * FROM `error_documents` WHERE 'code' == '" + response.Status.Code + "'";


        //}
    }
}

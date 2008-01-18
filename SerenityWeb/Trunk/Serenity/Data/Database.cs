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
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SQLite;
using System.Text;

namespace Serenity.Data
{
    public static class Database
    {
        public static DbConnection Open(DataScope scope)
        {
            throw new NotImplementedException();

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            
            SQLiteConnection con = new SQLiteConnection();
        }
    }
}

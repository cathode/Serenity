/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Data;

namespace Serenity.Users
{
    public class User
    {
        private string name;

        [DataObject("Name")]
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        [DataObject("Id")]
        public int UserId
        {
            get
            {
                return 0;
            }
        }
    }
}

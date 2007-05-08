/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Provides a WebDriver implementation which sends and recieves CommonRequests and CommonResponses
    /// in a binary format, using the BinaryAdapter.
    /// </summary>
    public class BinaryDriver : WebDriver
    {
        public BinaryDriver(ContextHandler contextHandler) : base(contextHandler)
        {
            this.Info = new DriverInfo("Serenity", "Serenity Transmission Protocol - Binary", "stp-b", new Version(0, 1));
        }
        protected override void DriverInitialize()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void DriverStart()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void DriverStop()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override WebAdapter CreateAdapter()
        {
            return new BinaryAdapter(this);
        }
    }
}

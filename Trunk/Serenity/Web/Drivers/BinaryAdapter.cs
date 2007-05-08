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
    public class BinaryAdapter : WebAdapter
    {
        internal BinaryAdapter(WebDriver Origin) : base(Origin)
        {

        }

        public override void ConstructRequest(byte[] source, out byte[] unused)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ConstructResponse(byte[] source, out byte[] unused)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override byte[] DestructResponse(CommonContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override byte[] DestructRequest(CommonContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

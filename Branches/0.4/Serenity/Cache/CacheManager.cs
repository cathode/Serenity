/*
Serenity - The next evolution of web server technology
Serenity/Cache/CacheManager.cs
Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serenity
{
    public static class CacheManager
    {
        static CacheManager()
        {
            System.Threading.Timer timer = new System.Threading.Timer(new System.Threading.TimerCallback(CacheManager.TimerCallback), null, CacheManager.CacheTimerFrequency, CacheManager.CacheTimerFrequency);
        }
        private static void TimerCallback(object value)
        {

        }
        /// <summary>
        /// Specifies the maximum number of bytes that can be used for a CacheStream.
        /// </summary>
        public const long MaxCacheCapacity = 65536;
        /// <summary>
        /// Specifies the maximum number of CacheStreams to keep open at once.
        /// </summary>
        public const int MaxCachedStreams = 64;
        /// <summary>
        /// Specifies the interval in milliseconds between cache ranking and cleanup.
        /// </summary>
        public const int CacheTimerFrequency = 5000;

    }
}

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

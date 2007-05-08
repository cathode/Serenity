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

namespace Serenity
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        static Singleton()
        {
        }
        protected Singleton()
        {
            lock (Singleton<T>.instance)
            {
                if (Singleton<T>.instance != null)
                {
                    throw new Exception("Cannot create duplicate instance of singleton class.");
                }
            }
        }
        private static T instance = new T();

        public static T Instance
        {
            get
            {
                return Singleton<T>.instance;
            }
        }
    }
}

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
    public abstract class ThreadSafetyMechanism<T> where T : ThreadSafetyMechanism<T>
    {
        #region Fields - Private
        private bool isNewInstance;
        #endregion
        #region Methods - Protected
        protected abstract T CreateSafeInstanceHelper();
        #endregion
        #region Methods - Public
        public T CreateSafeInstance()
        {
            if ((this.IsThreadSafe == true) || (this.isNewInstance == true))
            {
                this.isNewInstance = false;
                return (T)this;
            }
            else
            {
                T newInstance = this.CreateSafeInstanceHelper();
                newInstance.isNewInstance = true;
                return newInstance;
            }
        }
        #endregion
        #region Properties - Public
        public bool IsNewInstance
        {
            get
            {
                return this.isNewInstance;
            }
        }
        /// <summary>
        /// Gets a boolean value that indicates whether instance members of the current
        /// object have been implemented in a thread-safe manner.
        /// </summary>
        public virtual bool IsThreadSafe
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}

/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Properties;

namespace Serenity
{
    /// <summary>
    /// Provides a base class for objects which follow the multiton design pattern.
    /// </summary>
    /// <typeparam name="TKey">The type to be used for unique keys.</typeparam>
    /// <typeparam name="TValue">The type that will follow the multiton design pattern. This MUST be the inheriting type.</typeparam>
    public abstract class Multiton<TKey, TValue> : IDisposable where TValue : Multiton<TKey, TValue>
    {
        #region Constructors - Static
        /// <summary>
        /// Initializes the static members of the Multiton class.
        /// </summary>
        static Multiton()
        {
            Multiton<TKey, TValue>.instances = new Dictionary<TKey, TValue>();
        }
        #endregion
        #region Constructors - Protected
        /// <summary>
        /// Initializes a new instance of the Multiton class.
        /// </summary>
        /// <param name="key">The name of the instance.</param>
        protected Multiton(TKey key)
        {
            if (key != null)
            {
                lock (Multiton<TKey, TValue>.instances)
                {
                    if (Multiton<TKey, TValue>.GetInstance(key) == null)
                    {
                        this.key = key;
                        Multiton<TKey, TValue>.instances.Add(key, (TValue)this);
                        if (Multiton<TKey, TValue>.defaultInstance == null)
                        {
                            Multiton<TKey, TValue>.defaultInstance = (TValue)this;
                        }
                    }
                    else
                    {
                        throw new Exception(Resources.MultitonDuplicateKeyError);
                    }
                }
            }
            else
            {
                throw new Exception(Resources.MultitonNoKeyError);
            }
        }
        #endregion
        #region Fields - Private
        [ThreadStatic]
        private static TValue currentInstance;
        private static TValue defaultInstance;
        private static Dictionary<TKey, TValue> instances;
        private TKey key;
        private static TValue systemInstance;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Determines whether an instance with the specified key exists.
        /// </summary>
        /// <param name="key">The key of the instance to find.</param>
        /// <returns>True if the instance is found, false otherwise.</returns>
        public static bool ContainsInstance(TKey key)
        {
            lock (Multiton<TKey, TValue>.instances)
            {
                return Multiton<TKey, TValue>.instances.ContainsKey(key);
            }
        }
		/// <summary>
		/// Disposes any resources used by the current Multiton.
		/// </summary>
        public virtual void Dispose()
        {
            Multiton<TKey, TValue>.RemoveInstance(this.key);
        }
        /// <summary>
        /// Retrieves a Multiton instance.
        /// </summary>
        /// <param name="key">The key of the instance to retrieve.</param>
        /// <returns>The named instance, or null if no instance was found.</returns>
        public static TValue GetInstance(TKey key)
        {
            lock (Multiton<TKey, TValue>.instances)
            {
                if (Multiton<TKey, TValue>.instances.ContainsKey(key) == true)
                {
                    return Multiton<TKey, TValue>.instances[key];
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Removes an instance.
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveInstance(TKey key)
        {
            Multiton<TKey, TValue>.instances.Remove(key);
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the current Multiton instance for the currently executing thread.
        /// </summary>
        public static TValue CurrentInstance
        {
            get
            {
                if (Multiton<TKey, TValue>.currentInstance != null)
                {
                    return Multiton<TKey, TValue>.currentInstance;
                }
                else
                {
                    return Multiton<TKey, TValue>.DefaultInstance;
                }
            }
            set
            {
                Multiton<TKey, TValue>.currentInstance = value;
            }
        }
        /// <summary>
        /// Gets or sets the default 
        /// </summary>
        public static TValue DefaultInstance
        {
            get
            {
                if (Multiton<TKey, TValue>.defaultInstance != null)
                {
                    return Multiton<TKey, TValue>.defaultInstance;
                }
                else
                {
                    return Multiton<TKey, TValue>.SystemInstance;
                }
            }
            set
            {
                Multiton<TKey, TValue>.defaultInstance = value;
            }
        }
        /// <summary>
        /// Gets an array containing all instances of the current type.
        /// </summary>
        public static TValue[] Instances
        {
            get
            {
                TValue[] result = new TValue[Multiton<TKey, TValue>.instances.Count];
                Multiton<TKey, TValue>.instances.Values.CopyTo(result, 0);

                return result;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current Multiton is the Current instance.
        /// </summary>
        public bool IsCurrentInstance
        {
            get
            {
                if (this == Multiton<TKey, TValue>.CurrentInstance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current Multiton is the Default instance.
        /// </summary>
        public bool IsDefaultInstance
        {
            get
            {
                if (this == Multiton<TKey, TValue>.DefaultInstance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current Multiton is the System instance.
        /// </summary>
        public bool IsSystemInstance
        {
            get
            {
                if (this == Multiton<TKey, TValue>.SystemInstance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets the name of the current Multiton.
        /// </summary>
        public TKey Key
        {
            get
            {
                return this.key;
            }
        }
        /// <summary>
        /// Gets the system instance.
        /// </summary>
        public static TValue SystemInstance
        {
            get
            {
                if (Multiton<TKey, TValue>.systemInstance != null)
                {
                    return Multiton<TKey, TValue>.systemInstance;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Multiton<TKey, TValue>.systemInstance == null)
                {
                    if (value != null)
                    {
                        Multiton<TKey, TValue>.systemInstance = value;
                    }
                    else
                    {
                        throw new ArgumentException(Serenity.Properties.Resources.MultitonSystemInstanceNullError);
                    }
                }
                else
                {
                    throw new InvalidOperationException(Serenity.Properties.Resources.MultitonSystemInstanceRedefineError);
                }
            }
        }
        #endregion
    }
}
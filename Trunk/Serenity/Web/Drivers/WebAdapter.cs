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
    /// Provides a base class for Web Adapters,
    /// objects used to transform a CommonContext to or from an array of bytes.
    /// </summary>
    public abstract class WebAdapter
    {
        #region Constructors - Internal
        internal WebAdapter(WebDriver driver)
        {
            this.driver = driver;
            this.currentcontext = new CommonContext(driver);
            this.contexts = new Queue<CommonContext>();
        }
        #endregion
        #region Fields - Private
        private int available = 0;
        private Queue<CommonContext> contexts;
        private CommonContext currentcontext;
        private WebDriver driver;
        #endregion
        #region Methods - Protected
        /// <summary>
        /// Obsolete. Finalizes the CommonContext being constructed and adds it to
        /// the queue of available completed CommonContexts.
        /// </summary>
        /// <remarks>
        /// This method will be removed in Serenity 0.5.0.0.
        /// </remarks>
        [Obsolete]
        protected void Archive()
        {
            this.Finalize();
        }
        /// <summary>
        /// Finalizes the CommonContext being constructed and adds it to
        /// the queue of available completed CommonContexts.
        /// </summary>
        protected void Finalize()
        {
            if (this.currentcontext != null)
            {
                this.available++;
                this.contexts.Enqueue(this.currentcontext);
                this.currentcontext = new CommonContext(this.driver);
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// When overridden in a derived class, translates the input Byte array to one or more CommonRequest objects.
        /// </summary>
        /// <param name="source">The bytes used as input.</param>
        /// <param name="unused">Any bytes that were unable to be processed or were unnecessary.</param>
        public abstract void ConstructRequest(Byte[] source, out Byte[] unused);
        /// <summary>
        /// When overridden in a derived class, translates the input Byte array to one or more CommonResponse objects.
        /// </summary>
        /// <param name="source">The bytes used as input.</param>
        /// <param name="unused">Any bytes that were unable to be processed or were unnecessary.</param>
        public abstract void ConstructResponse(Byte[] source, out Byte[] unused);
        /// <summary>
        /// When overridden in a derived class,
        /// translates the CommonResponse of context to an array of bytes.
        /// </summary>
        /// <param name="context">The CommonContext to translate.</param>
        /// <returns>An array of bytes representing the CommonResponse of context.</returns>
        public abstract Byte[] DestructResponse(CommonContext context);
        /// <summary>
        /// When overridden in a derived class,
        /// translates the CommonRequest of context to an array of bytes.
        /// </summary>
        /// <param name="context">The CommonContext to translate.</param>
        /// <returns>An array of bytes representing the CommonRequest of context.</returns>
        public abstract Byte[] DestructRequest(CommonContext context);
        /// <summary>
        /// If there are available contexts in the postprocess queue, gets the next available CommonContext.
        /// If there are none available, returns null.
        /// </summary>
        /// <returns>The next available CommonContext, or null if none available.</returns>
        public CommonContext NextContext()
        {
            if (this.available > 0)
            {
                this.available--;
                return this.contexts.Dequeue();
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region Properties - Protected
        /// <summary>
        /// Gets the CommonContext currently being constructed.
        /// </summary>
        protected CommonContext CurrentContext
        {
            get
            {
                return this.currentcontext;
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the number of CommonContexts available.
        /// </summary>
        public int Available
        {
            get
            {
                return this.available;
            }
        }
        /// <summary>
        /// Gets the WebDriver which created the current WebAdapter.
        /// </summary>
        public WebDriver Driver
        {
            get
            {
                return this.driver;
            }
        }
        /// <summary>
        /// Obsolete. Gets the WebDriver which created the current WebAdapter.
        /// </summary>
        /// <remarks>
        /// This property will be removed in Serenity 0.5.0.0.
        /// </remarks>
        [Obsolete]
        public WebDriver Origin
        {
            get
            {
                return this.driver;
            }
        }
        #endregion
    }
}
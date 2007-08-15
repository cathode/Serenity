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

namespace Serenity
{
    internal sealed class DomainSettingValue<T>
    {
        #region Constructors - Internal
        internal DomainSettingValue()
            : this(null)
        {
            this.value = default(T);
        }
        internal DomainSettingValue(DomainSettingValue<T> parent)
        {
            if (parent == null)
            {
                this.hasParent = false;
            }
            else
            {
                this.parent = parent;
            }
        }
        #endregion
        #region Fields - Private
        private DomainSettingValue<T> parent;
        private T value;
        #endregion
        #region Fields - Internal
        internal bool isDefined;
        internal bool hasParent;
        #endregion
        #region Properties - Public
        internal bool IsDefined
        {
            get
            {
                return this.isDefined;
            }
        }
        internal DomainSettingValue<T> Parent
        {
            get
            {
                return this.parent;
            }
			set
			{
				this.parent = value;
			}
        }
        public T Value
        {
            get
            {
                if (this.isDefined == true)
                {
                    return this.value;
                }
                else if (this.hasParent == true)
                {
                    return this.parent.Value;
                }
                else
                {
                    return default(T);
                }
            }
            set
            {
                this.isDefined = true;
                this.value = value;
            }
        }
        #endregion
    }
}

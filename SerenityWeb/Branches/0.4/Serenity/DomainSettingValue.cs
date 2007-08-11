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

namespace Serenity
{
    [Serializable]
    public sealed class DomainSettingValue<T>
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
        [NonSerialized]
        private readonly DomainSettingValue<T> parent;
        private T value;
        #endregion
        #region Fields - Internal
        [NonSerialized]
        internal bool isDefined;
        [NonSerialized]
        internal bool hasParent;
        #endregion
        #region Properties - Public
        public bool IsDefined
        {
            get
            {
                return this.isDefined;
            }
        }
        public DomainSettingValue<T> Parent
        {
            get
            {
                return this.parent;
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

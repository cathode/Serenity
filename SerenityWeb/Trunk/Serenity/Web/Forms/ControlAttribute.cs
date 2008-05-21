using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Represents an attribute of a <see cref="Control"/>.
    /// </summary>
    public sealed class ControlAttribute
    {
        #region Fields - Private
        private string name;
        private string value;
        private bool include;
        #endregion
        #region Properties - Public
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
        public bool Include
        {
            get
            {
                return this.include;
            }
            set
            {
                this.include = value;
            }
        }
        #endregion
    }
}

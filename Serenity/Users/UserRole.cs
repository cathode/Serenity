using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Users
{
    /// <summary>
    /// Represents a user account role
    /// </summary>
    public class UserRole
    {
        #region Properties
        public Guid UniqueID
        {
            get;
            set;
        }

        public UserActions EnabledActions
        {
            get;
            set;
        }
        #endregion
    }
}

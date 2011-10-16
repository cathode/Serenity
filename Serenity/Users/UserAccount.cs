using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Users
{
    /// <summary>
    /// Represents a user's account information.
    /// </summary>
    public class UserAccount
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }

        public byte[] PasswordHash
        {
            get;
            set;
        }

        public Guid UniqueID
        {
            get;
            set;
        }

        public Guid[] Roles
        {
            get;
            set;
        }
        #endregion
    }
}

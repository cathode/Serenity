using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Users
{
    /// <summary>
    /// Represents actions a user can perform.
    /// </summary>
    [Flags]
    public enum UserActions
    {
        None = 0x0,
        CreateUser,
        ModifyUser,
        RemoveUser,
        EnableUser,
        DisableUser,
        AddWebApp,
        RemoveWebApp,
        EnableWebApp,
        DisableWebApp,
    }
}

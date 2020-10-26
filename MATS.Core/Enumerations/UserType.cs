using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MATS.Core.Enumerations
{
    public enum UserType
    {
        [Description("Administrator")]
        Administrator,       
        [Description("Myco User")]
        MycoUser,
        [Description("Client User")]
        ClientUser

    }
    public enum UserTypes
    {
        Waiting = 0,
        Active = 1,
        Disabled = 2,
        Blocked = 3
    }
}

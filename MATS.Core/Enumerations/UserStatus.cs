using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MATS.Core.Enumerations
{
    public enum UserStatus
    {
        [Description("Active User")]
        Active,
        [Description("Block User")]
        Blocked,
        [Description("Remove User")]
        Removed
    }

}
